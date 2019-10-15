using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.StatusDesembaracoSefaz.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"http://online.sefaz.am.gov.br/diselada/consultadi.asp";

        private string _urlApiBase;
        private List<Importacao> registros;
        int _cd_bot_exec;
        int _cd_par;
        #endregion

        public Work()
        {
            LogController.RegistrarLog("#################  INICIALIZANDO - STATUS DE DESEMBARAÇO DA SEFAZ  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public Work(int cd_bot_exec, int cd_par)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;

            LogController.RegistrarLog("#################  INICIALIZANDO - STATUS DE DESEMBARAÇO DA SEFAZ  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog("Obtendo DI's para Consultar Status de Desembaraço na Sefaz.", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/status-desembaraco/" + _cd_par;

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
                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            try
                            {
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
                    LogController.RegistrarLog("Não existe DI's para Acompanhar Status.", eTipoLog.INFO, _cd_bot_exec, "bot");
                    //Console.ReadKey();
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            var numAno = numero.Substring(0,2);
            var numDeclaracao = numero.Substring(2, 7);
            var numDigito = numero.Substring(9, 1);

            LogController.RegistrarLog("ACESSANDO SITE...", eTipoLog.INFO, _cd_bot_exec, "bot");
            _driver.Navigate().GoToUrl(_urlSite);

            LogController.RegistrarLog("Inserindo o Ano...", eTipoLog.INFO, _cd_bot_exec, "bot");
            //COLOCANDO O ANO
            OpenQA.Selenium.IWebElement element = _driver.FindElementById("txtAno");
            element.SendKeys("20"+numAno);

            LogController.RegistrarLog("Inserindo o Numero...", eTipoLog.INFO, _cd_bot_exec, "bot");
            //COLOCANDO O NUMERO
            element = _driver.FindElementById("txtNumeroDI");
            element.SendKeys(numDeclaracao);

            LogController.RegistrarLog("Inserindo o Digito Verificador...", eTipoLog.INFO, _cd_bot_exec, "bot");
            //COLOCANDO O NUMERO DO DIGITO VERIFICADOR
            element = _driver.FindElementById("txtDigito");
            element.SendKeys(numDigito);

            // clica no BOTAO 'Confirmar'
            element = _driver.FindElementById("pesquisar");
            element.Click();

            LogController.RegistrarLog("Gravando o Screenshot da Tela de Consulta...", eTipoLog.INFO, _cd_bot_exec, "bot");

            var retorno = capturaImagem(_driver, numero);

            LogController.RegistrarLog("Gravando Status...", eTipoLog.INFO, _cd_bot_exec, "bot");

            import.OP_STATUS_DESEMB = retorno ? 1 : 0;
            await AtualizaStatusDesembaraco(import, nroDI);

            LogController.RegistrarLog("Concluído !!!", eTipoLog.INFO, _cd_bot_exec, "bot");
        }

        public void Screenshot(IWebDriver driver, string screenshotsPasta)
        {
            ITakesScreenshot camera = driver as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile(screenshotsPasta, ScreenshotImageFormat.Png);
        }

        public bool capturaImagem(PhantomJSDriver _driver, string numero)
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", numero + "-CapturaTela.jpg");

                Screenshot(_driver, arquivoPath);
                Thread.Sleep(500);

                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        private async Task AtualizaStatusDesembaraco(Importacao import, string cd_imp)
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
    }
}
