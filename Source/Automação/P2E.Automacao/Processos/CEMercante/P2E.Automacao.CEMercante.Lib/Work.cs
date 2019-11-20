using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using Selenium.Utils.Html;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.CEMercante.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www.mercante.transportes.gov.br/g36127/servlet/serpro.siscomex.mercante.servlet.MercanteController";

        private string _urlApiBase;

        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;

        #endregion

        public Work()
        {
            LogController.RegistrarLog("#################  INICIALIZANDO - CE MERCANTE  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog("#################  INICIALIZANDO - CE MERCANTE  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            //LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's ", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService())
            {
                LogController.RegistrarLog("Carregando certificado...", eTipoLog.INFO, _cd_bot_exec, "bot");
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
                {
                    try
                    {
                        Acessar(_driver);

                        LogController.RegistrarLog($"Execução concluída.", eTipoLog.INFO, _cd_bot_exec, "bot");
                    }
                    catch (Exception)
                    {
                        _driver.Close();
                    }
                }
            }
        }

        private async Task Acessar(PhantomJSDriver _driver)
        {
            try
            {
                LogController.RegistrarLog("Acessando URL...", eTipoLog.INFO, _cd_bot_exec, "bot");

                _driver.Navigate().GoToUrl(_urlSite);
                Thread.Sleep(500);

                LogController.RegistrarLog($"Clique na Area para Cadastrados", eTipoLog.INFO, _cd_bot_exec, "bot");
                OpenQA.Selenium.IWebElement element = _driver.FindElementByCssSelector("body > form:nth-child(3) > table:nth-child(6) > tbody > tr > td:nth-child(4) > p:nth-child(6) > a");
                element.Click();
                Thread.Sleep(500);

                LogController.RegistrarLog($"Click no Certificado", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElementByCssSelector("#cpfsenha > table.Tab1 > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(4) > td:nth-child(1) > table > tbody > tr:nth-child(4) > td:nth-child(2) > div > a > img");
                element.Click();
                Thread.Sleep(500);

                var retorno = capturaImagem(_driver, "0");

                LogController.RegistrarLog($"Clique na Aba 'Conhecimento'", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElementByXPath("/html/body/form[1]/table[2]/tbody/tr/td[2]/a/img");
                element.Click();
                Thread.Sleep(500);

                retorno = capturaImagem(_driver, "1");

                LogController.RegistrarLog($"Selecionando: Conhecimento/BL/BL-HOUSE", eTipoLog.INFO, _cd_bot_exec, "bot");
                Select selectTipo = new Select(_driver, By.Name("cmbAcoes"));
                selectTipo.SelectByText("Conhecimento/BL/BL-HOUSE");
                Thread.Sleep(500);

                retorno = capturaImagem(_driver, "2");

                LogController.RegistrarLog($"Selecionando: Consultar Conhecimentos Incluídos e Alterados", eTipoLog.INFO, _cd_bot_exec, "bot");
                selectTipo = new Select(_driver, By.Name("cmbAcoesNivel2"));
                selectTipo.SelectByText("Consultar Conhecimentos Incluídos e Alterados");
                Thread.Sleep(500);

                retorno = capturaImagem(_driver, "3");










                //

                ////COLOCANDO O ANO
                //
                //element.SendKeys("20" + numAno);

                //LogController.RegistrarLog(_nome_cliente + " - " + "Inserindo o Numero...", eTipoLog.INFO, _cd_bot_exec, "bot");
                ////COLOCANDO O NUMERO
                //element = _driver.FindElementById("txtNumeroDI");
                //element.SendKeys(numDeclaracao);

                //LogController.RegistrarLog(_nome_cliente + " - " + "Inserindo o Digito Verificador...", eTipoLog.INFO, _cd_bot_exec, "bot");
                ////COLOCANDO O NUMERO DO DIGITO VERIFICADOR
                //element = _driver.FindElementById("txtDigito");
                //element.SendKeys(numDigito);

                //// clica no BOTAO 'Confirmar'
                //element = _driver.FindElementById("pesquisar");
                //element.Click();




                //retorno = capturaImagem(_driver, "1");
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em Acessar. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
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

                string arquivoPath = Path.Combine(@"C:\Versatilly\", numero + "-CapturaTelaCE.jpg");

                Screenshot(_driver, arquivoPath);
                Thread.Sleep(1000);

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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
