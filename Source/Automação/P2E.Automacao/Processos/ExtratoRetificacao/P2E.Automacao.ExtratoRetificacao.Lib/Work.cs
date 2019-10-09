using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.ExtratoRetificacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarExtratoRetificacaoMenu.do";
        public string _urlDownload = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarExtratoRetificacao.do";

        private string _urlApiBase;
        private List<Importacao> registros;
        int _cd_bot_exec;
        #endregion

        public Work()
        {
            LogController.RegistrarLog("#################  INICIALIZANDO - EXTRATO DE RETIFICAÇÃO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public Work(int cd_bot_exec)
        {
            _cd_bot_exec = cd_bot_exec;
            LogController.RegistrarLog("#################  INICIALIZANDO - EXTRATO DE RETIFICAÇÃO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog("Obtendo DI's para Consultar Extrato de Retificação.", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-retif";

            using (var client = new HttpClient())
            {
                LogController.RegistrarLog("ABRINDO CONEXAO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                var result = await client.GetAsync(urlAcompanha);
                var aux = await result.Content.ReadAsStringAsync();
                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService())
                    {
                        LogController.RegistrarLog("CARREGANDO O CERTIFICADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        ControleCertificados.CarregarCertificado(service);

                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            try
                            {
                                LogController.RegistrarLog("ACESSANDO SITE...", eTipoLog.INFO, _cd_bot_exec, "bot");
                                _driver.Navigate().GoToUrl(_urlSite);

                                foreach (var di in registros)
                                {
                                    LogController.RegistrarLog("################# DI: " + di.TX_NUM_DEC + " #################", eTipoLog.INFO, _cd_bot_exec, "bot");

                                    List<Thread> threads = new List<Thread>();

                                    var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver, di, di.CD_IMP.ToString()));
                                    thread.Start();
                                    threads.Add(thread);

                                    // fica aguardnado todas as threads terminarem...
                                    while (threads.Any(t => t.IsAlive))
                                    {
                                        continue;
                                    }
                                }

                                //Console.ReadKey();
                            }
                            catch (Exception)
                            {
                                _driver.Close();
                            }
                        }
                    }
                }
                else
                {
                    LogController.RegistrarLog("Não existe DI's para Acompanhar Despacho.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            //using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))

            var numDeclaracao = numero;

            LogController.RegistrarLog("ACESSAO PAGINA DE CONSULTA...", eTipoLog.INFO, _cd_bot_exec, "bot");
            _driver.Navigate().GoToUrl(_urlTelaConsulta);

            //COLOCANDO O NUMERO DA DI NO CAMPO
            OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
            element.SendKeys(numDeclaracao);

            // clica no BOTAO 'Confirmar'
            element = _driver.FindElementByCssSelector("#corpo > input:nth-child(3)");
            element.Click();

            //VERIFICA SE TEM POPUP.. PRECISA MELHORAR...
            try { var aux = element.Text; }
            catch (Exception)
            {
                import.OP_EXTRATO_RETIF = 0;

                await AtualizaExtratoRetificacao(import, nroDI); 

                LogController.RegistrarLog("DI NÃO RETIFICADA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                return;
            }

            string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                                        numDeclaracao.Substring(2, 7) + "-" +
                                        numDeclaracao.Substring(9, 1);

            LogController.RegistrarLog("DOWNLOAD DO EXTRATO DE RETIFICACAO EM PDF...", eTipoLog.INFO, _cd_bot_exec, "bot");
            var retornoRetif = DownloadComprovante(_driver, _urlDownload + "?nrDeclaracao=" + numeroDec);

            import.OP_EXTRATO_RETIF = retornoRetif ? 1 : 0;

            await AtualizaExtratoRetificacao(import, nroDI);
        }

        private async Task AtualizaExtratoRetificacao(Importacao import, string cd_imp)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);
                    resultado.EnsureSuccessStatusCode();

                    LogController.RegistrarLog("Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog($"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
        }

        protected bool DownloadComprovante(PhantomJSDriver driver, string _url)
        {
            try
            {
                //var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");
                var certificado = ControleCertificados.GetClientCertificate();

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-ExtratoRetificacao.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
