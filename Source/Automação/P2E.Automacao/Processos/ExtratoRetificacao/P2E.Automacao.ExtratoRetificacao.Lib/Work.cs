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
        int _cd_par;
        string _nome_cliente;

        #endregion

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - EXTRATO DE RETIFICAÇÃO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - EXTRATO DE RETIFICAÇÃO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para Consultar Extrato de Retificação.", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-retif/" + _cd_par;

            using (var client = new HttpClient())
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "ABRINDO CONEXAO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                var result = await client.GetAsync(urlAcompanha);
                var aux = await result.Content.ReadAsStringAsync();
                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService())
                    {
                        LogController.RegistrarLog(_nome_cliente + " - " + "CARREGANDO O CERTIFICADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        ControleCertificados.CarregarCertificado(service);

                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        var options = new PhantomJSOptions();

                        using (var _driver = new PhantomJSDriver(service, options, TimeSpan.FromMinutes(2)))
                        {
                            try
                            {
                                LogController.RegistrarLog(_nome_cliente + " - " + "ACESSANDO SITE...", eTipoLog.INFO, _cd_bot_exec, "bot");
                                _driver.Navigate().GoToUrl(_urlSite);

                                foreach (var di in registros)
                                {
                                    LogController.RegistrarLog(_nome_cliente + " - DI: " + di.TX_NUM_DEC + " - ################# DI: " + di.TX_NUM_DEC + " #################", eTipoLog.INFO, _cd_bot_exec, "bot");

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
                            catch (Exception ex)
                            {
                                LogController.RegistrarLog(_nome_cliente + " - " + "ERRO CONEXAO: " + ex.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");
                                _driver.Close();
                            }
                        }
                    }
                }
                else
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + " Não existe DI's para Acompanhar Despacho.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            var numDeclaracao = numero;

            try
            {

                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " - ACESSAO PAGINA DE CONSULTA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                _driver.Navigate().GoToUrl(_urlTelaConsulta);

                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " - ADICIONANDO NUMERO DA DI...", eTipoLog.INFO, _cd_bot_exec, "bot");
                OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
                element.SendKeys(numDeclaracao);

                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " - CLICANDO NO BOTÃO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElementByCssSelector("#corpo > input:nth-child(3)");
                element.Click();

                //VERIFICA SE TEM POPUP.. PRECISA MELHORAR...
                try { var aux = element.Text; }
                catch (Exception)
                {
                    import.OP_EXTRATO_RETIF = 0;

                    await AtualizaExtratoRetificacao(import, nroDI);

                    LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + "DI NÃO RETIFICADA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    return;
                }

                string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                                            numDeclaracao.Substring(2, 7) + "-" +
                                            numDeclaracao.Substring(9, 1);

                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + "DOWNLOAD DO EXTRATO DE RETIFICACAO EM PDF...", eTipoLog.INFO, _cd_bot_exec, "bot");
                var retornoRetif = DownloadComprovante(_driver, _urlDownload + "?nrDeclaracao=" + numeroDec);

                import.OP_EXTRATO_RETIF = retornoRetif ? 1 : 0;

                await AtualizaExtratoRetificacao(import, nroDI);
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + "ERRO - " + ex.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }

        private async Task AtualizaExtratoRetificacao(Importacao import, string cd_imp)
        {
            try
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + cd_imp + " ATUALIZANDO RETIFICACAO...", eTipoLog.INFO, _cd_bot_exec, "bot");

                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado =  client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import).Result;
                    resultado.EnsureSuccessStatusCode();

                    LogController.RegistrarLog(_nome_cliente + " - DI: " + cd_imp + " Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + cd_imp + $" Erro ao atualizar a DI nº {import.TX_NUM_DEC}.", eTipoLog.ERRO, _cd_bot_exec, "bot");
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
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"+ _nome_cliente + "\\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\"+ _nome_cliente + "\\");
                }

                string arquivoPath = Path.Combine(@"C:\Versatilly\"+ _nome_cliente + "\\", horaData + "-ExtratoRetificacao.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_url, arquivoPath);

                    Thread.Sleep(5000);
                }

                LogController.RegistrarLog(_nome_cliente + " - " +  " DOWNLOAD PDF CONCLUIDO", eTipoLog.INFO, _cd_bot_exec, "bot");

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - "+ "ERRO DOWNLOAD - " + e.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");
                return false;
            }
        }
    }
}
