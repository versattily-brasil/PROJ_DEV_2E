using OpenQA.Selenium.PhantomJS;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace StatusDespacho
{
    public class StatusDespachos
    {
        private StreamWriter writer;
        private string path;
        private bool checkedValue;

        public StatusDespachos( bool checkedValue)
        {
            this.writer = null;
            this.path = path;
            this.checkedValue = checkedValue;
        }

        public void executaBat()
        {
            Process.Start(path);
        }

        public bool Executar()
        {
            // url utilizadas no procedimento
            string urlInicioPrivado =  @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
            string urlAcompanhaDespacho = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/AcompanharSituacaoDespachoMenu.do";

            try
            {
                // inicializa o phantonjs
                using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
                {
                    // preenche informações sobre o certificado.
                    service.IgnoreSslErrors = true;
                    string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.crt";
                    service.AddArgument(cert);
                    service.AddArgument($"--ssl-client-key-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.key");
                    service.AddArgument($"--ssl-client-key-passphrase=2e123456$");

                    service.AddArgument("test-type");
                    service.AddArgument("no-sandbox");
                    service.HideCommandPromptWindow = true;

                    var options = new PhantomJSOptions();
                    options.AddAdditionalCapability("handlesAlerts", true);

                    // inicia navegação
                    using (var _driver = new PhantomJSDriver( service, options))
                    {
                        // navega para a primeira url
                        // nesse momento, é realizado o login através do certificado
                        // se esse procedimento for feito manualmente atraves do navegador, aparecerá a tela para selecionar o certiifcado
                        // mas nesse caso, ele já seleciona automatico sem necessidade de intervenção 
                        _driver.Navigate().GoToUrl(urlInicioPrivado);

                        /// navega para a url de declarãção de icms
                        _driver.Navigate().GoToUrl(urlAcompanhaDespacho);

                        // localiza o elemento na pagina com o id = login
                        OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");

                        // preenche o txt senha com o valor da senha
                        element.SendKeys("1915359807");//COLOCAR DENTRO DO FOREACH PRA PEGAR TODOS QUE NAO ESTÃO COM STATUS DE DESEMBARAÇO

                        // clica no link contendo o 'Consultar'
                        element = _driver.FindElementByCssSelector(@"#botoes > input:nth-child(1)");
                        element.Click();

                        // clica no link contendo o ...
                        element = _driver.FindElementByPartialLinkText("19/1535980-7");
                        element.Click();

                        //




                        // localiza o elemento na pagina
                        element = _driver.FindElementById("DECLARACAO DESEMBARACADA");

                        //FAZER UM IF PRA SABER SE VEIO DESEMBARACADO, SE VEIO FAZER UPDATE NO BANCO



                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
