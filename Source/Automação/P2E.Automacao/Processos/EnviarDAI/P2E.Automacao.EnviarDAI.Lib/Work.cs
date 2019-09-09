using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.EnviarDAI.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = "http://sistemas.sefaz.am.gov.br/GDDI/publico/uploadDiXmlAction.do";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        #endregion
        public void Executar()
        {
            Acessar();
        }

        protected void Acessar()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                // carrega o cerificado.. retirar se não for necessário.
                //OBS.: ver necessidade do certicadfo
                //ControleCertificados.CarregarCertificado(service);


                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("login");

                    //var credencial = ObterCredenciaisSuframa();

                    //Localiza o caminho e o nome do arquivo
                    element = _driver.FindElementByName("theFIle");
                    element.SendKeys("@C:\\temp\\DAI.xml");

                    //localiza e clica no botão Enviar DAI
                    element = _driver.FindElementById("confirm");
                    element.Click();

                }
            }
        }

        //private CredenciaisSuframa ObterCredenciaisSuframa()
        //{
        //    // Obter credenciais para acesso ao site
        //    return new CredenciaisSuframa() { Usuario = "08281892000158", Senha = "2edespachos" };
        //}

    }
}
