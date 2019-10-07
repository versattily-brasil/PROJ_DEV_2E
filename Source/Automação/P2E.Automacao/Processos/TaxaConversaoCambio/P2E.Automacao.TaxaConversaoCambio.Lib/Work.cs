using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.TaxaConversaoCambio.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www35.receita.fazenda.gov.br/tabaduaneiras-web/public/pages/security/login_publico.jsf";

        private string _urlApiBase;
        private List<Importacao> registros;
        private string arquivoPath = "";
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - TAXA DE CONVERSÃO DE CAMBIO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            Console.WriteLine("CONFIGURANDO AMBIENTE...");
            using (var client = new HttpClient())
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
                            Acessar(_driver);

                            Console.ReadKey();
                        }
                        catch (Exception)
                        {
                            _driver.Close();
                        }
                    }
                }

            }
        }

        private async Task Acessar(PhantomJSDriver _driver)
        {
            try
            {
                Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
                _driver.Navigate().GoToUrl(_urlSite);

                Console.WriteLine("PREPARANDO SITE...");
                Console.WriteLine("CAPTURANDO STRING BASE64 DA IMAGEM...");

                var aux = _driver.PageSource;
                var auxInicial = aux.IndexOf("image/png;base64,");
                var auxFinal = aux.IndexOf("divButtons_captcha_serpro_gov_br");

                var lenghtImage = ((auxFinal-11) - (auxInicial + 17));

                var image64 = aux.Substring(auxInicial+17, lenghtImage);   //AzCaptcha.ConverteImageParaBase64Url(_urlSite);

                Console.WriteLine("OBTENDO O VALOR DO CAPTCHA...");
                var valorCaptcha = AzCaptcha.ResultadoCaptcha(image64, "hnwrvh9xdjyp2kcyqfnd7bmwmf3qkt8v");

                Console.WriteLine("INSERINDO VALOR DO CAPTCHA NO BROWSER...");
                IWebElement element = _driver.FindElementById("txtTexto_captcha_serpro_gov_br");

                Console.WriteLine("inserindo o numero da declaração");
                element.SendKeys(valorCaptcha);

                element = _driver.FindElementByCssSelector(@"#j_id11 > div:nth-child(4) > center > input");
                element.Click();

                Thread.Sleep(5000);

                if(_driver.PageSource.Contains("Texto digitado não corresponde a imagem"))
                {
                    Console.WriteLine("Texto digitado não corresponde a imagem");                    
                }
                else
                {
                    //CLICA EM TAXA DE CONVERSAO DE CAMBIO
                    element = _driver.FindElement(By.CssSelector("#j_id110\\:agrupamento\\:13\\:grupo\\:0\\:j_id121 > center > a"));
                    element.Click();

                    Thread.Sleep(5000);

                    //CLICA EM TODOS
                    element = _driver.FindElement(By.Id(@"j_id111:j_id163"));
                    element.Click();

                    Thread.Sleep(5000);

                    //CLICA NO EXTRAIR TABELA
                    element = _driver.FindElement(By.Id(@"j_id111:j_id207"));
                    element.Click();

                    Thread.Sleep(5000);

                    //DOWNLOAD
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro-" + e.Message.Trim());
                _driver.Close();
                Console.ReadKey();
            }
            

        }
    }
}
