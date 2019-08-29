using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnvioPLI.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = "https://portal1.suframa.gov.br/pmerecebimento/rotinas.do";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        #endregion
        public void Executar()
        {
            // Obter processos não registrados

            // Obter credenciais para acesso ao site
            _loginSite = "08281892000158";
            _senhaSite = "2edespachos";
            // Gerar Arquivo PLI ?

            // Preparar Arquivo PLI para envio

            // Enviar arquivo ZIP
            EnviarArquivo();

            // Recuperar Erros ocorrerem

            // Registrar Execução

            // Registrar Erros
        }

        protected void EnviarArquivo()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("login");

                    element.SendKeys(_loginSite);

                    element = _driver.FindElementByName("field(-senha)");

                    element.SendKeys(_senhaSite);


                    element = _driver.FindElementByName("btLogar");
                    element.Click();

                    // etapa 2
                    element = _driver.FindElementByPartialLinkText("Enviar PLI");
                    element.Click();

                    // etapa 3
                    element = _driver.FindElementByName("field(-manter-arquivo)");
                    element.SendKeys(ObterArquivoTemporario());
                    element = _driver.FindElementById("btnEnviarpli");
                    element.Click();
                    // etapa 4
                    element = _driver.FindElementById("ERROR");

                    Console.WriteLine(element.Text);
                }
            }
        }

        private string ObterArquivoTemporario()
        {
            string temp = Path.GetTempPath() + "\\temp.txt";

            if (!File.Exists(temp))
            {
                StreamWriter writer = new StreamWriter(temp);
                writer.WriteLine("Teste Envio PLI." + DateTime.Now.ToString());
                writer.Close();
            }

            return temp;
        }
    }
}
