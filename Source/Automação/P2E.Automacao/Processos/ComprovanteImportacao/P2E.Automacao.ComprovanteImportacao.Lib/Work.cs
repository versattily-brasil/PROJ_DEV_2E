using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Processos.ComprovanteImportacao.Lib;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.ComprovanteImportacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/RecuperarComprovanteMenu.do";
        public string _urlImprimir = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/ImprimirComprovante.do";

        private string _urlApiBase;
        private List<Importacao> registros;
        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;

        #endregion

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - COMPROVANTE DE IMPORTACAO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - COMPROVANTE DE IMPORTACAO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot"); 
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para Download de Comprovante.", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/comprovante-imp/" + _cd_par;

            using (var client = new HttpClient())
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "Abrindo conexão...", eTipoLog.INFO, _cd_bot_exec, "bot"); 
                var result = await client.GetAsync(urlAcompanha);
                string aux = await result.Content.ReadAsStringAsync();
                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService())
                    {
                        LogController.RegistrarLog(_nome_cliente + " - " + "Carregando certificado...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        ControleCertificados.CarregarCertificado(service);

                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            _driver.Navigate().GoToUrl(_urlSite);

                            try
                            {
                                foreach (var di in registros)
                                {
                                   LogController.RegistrarLog(_nome_cliente + " - " + $"Processando DI: {di.TX_NUM_DEC}", eTipoLog.INFO, _cd_bot_exec, "bot");

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

                               LogController.RegistrarLog(_nome_cliente + " - " + $"Execução concluída.", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                   LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's para Acompanhar Despacho.", eTipoLog.ERRO, _cd_bot_exec, "bot");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            try
            {
                //using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))

                var numDeclaracao = numero;

                LogController.RegistrarLog(_nome_cliente + " - " + "Acessando URL...", eTipoLog.INFO, _cd_bot_exec, "bot");
                
                _driver.Navigate().GoToUrl(_urlTelaConsulta);

                //COLOCANDO O NUMERO DA DI NO CAMPO
                OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
                element.SendKeys(numDeclaracao);

                // clica no BOTAO 'Confirmar'
                element = _driver.FindElementById("confirmar");
                element.Click();

                string Numero = numDeclaracao.Substring(0, 2) + "/" +
                                numDeclaracao.Substring(2, 7) + "-" +
                                numDeclaracao.Substring(9, 1);

                string id_tr = "tr_" + Numero;
                element = _driver.FindElementById(id_tr);

                //CAPTURA STATUS
                var status = element.Text;

                if (status.Contains("COMPROVANTE JA EMITIDO. UTILIZAR EMISSAO SEGUNDA VIA"))
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + status, eTipoLog.INFO, _cd_bot_exec, "bot");
                    // clica no botao OK
                    element = _driver.FindElementByCssSelector(@"#botoes > input");
                    element.Click();

                    // clica no radiobutton 2º via
                    element = _driver.FindElementByCssSelector(@"#corpo > fieldset:nth-child(1) > div > input[type=radio]:nth-child(2)");
                    element.Click();

                    element = _driver.FindElementById("nrDeclaracao");
                    element.SendKeys(numDeclaracao);

                    // clica no BOTAO 'Confirmar'
                    element = _driver.FindElementById("confirmar");
                    element.Click();
                }
                else if (status.Contains("DECLARACAO NAO ESTA DESEMBARACADA."))
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + status, eTipoLog.INFO, _cd_bot_exec, "bot");

                    import.OP_COMPROVANTE_IMP = 0;

                    await AtualizaComprovante(import, nroDI);

                    return;
                }

                id_tr = "tr_" + Numero;

                element = _driver.FindElementById(id_tr);
                status = element.Text;

                if (status.Contains("COMPROVANTE RECUPERADO COM SUCESSO"))
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + status, eTipoLog.INFO, _cd_bot_exec, "bot");

                    string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                                numDeclaracao.Substring(2, 7) + "-" +
                                numDeclaracao.Substring(9, 1);

                    LogController.RegistrarLog(_nome_cliente + " - " + "DOWNLOAD DE COMPROVANTE PDF...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    var retornFile = DownloadComprovante(_driver, _urlImprimir + "?nrDeclaracao=" + numeroDec);

                    import.OP_COMPROVANTE_IMP = retornFile ? 1 : 0;

                    await AtualizaComprovante(import, nroDI);
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em Acessar. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
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

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-COMPROVANTE_DI.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em DownloadComprovante. {e.Message}.", eTipoLog.ERRO, _cd_bot_exec, "bot");
                return false;
            }
        }

        private async Task AtualizaComprovante(Importacao import, string cd_imp)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);
                    resultado.EnsureSuccessStatusCode();

                   LogController.RegistrarLog(_nome_cliente + " - " + "Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
            catch (Exception e)
            {
               LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em AtualizaComprovante ao atualizar a DI nº {import.TX_NUM_DEC}.", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }
    }
}
