using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Processos.EnviarPLI.Lib.Entidades.EstruturaPLI;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO.Compression;

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
            //string temp = Path.GetTempPath() + "\\temp.txt";

            //if (!File.Exists(temp))
            //{
            //    StreamWriter writer = new StreamWriter(temp);
            //    writer.WriteLine("Teste Envio PLI." + DateTime.Now.ToString());
            //    writer.Close();
            //}

            //return temp;
            return GerarArquivoTeste();
        }

        private string GerarArquivoTeste()
        {
            #region Obtem dados da PLI
            var importador = new Importador();
            importador.tipoImportador = 2;
            importador.cdImportador = 12345678900014;

            var item1 = new ItemMatriz();
            item1.cdNcmProdFinal = 12345678;
            item1.cdSuframa = 1234;
            item1.cdDestinacao = 12;
            item1.cdUtilizacao = 34;
            item1.cdTributacao = "ABCD";
            item1.numDecreto = 888882019;
            item1.dtInicioBeneficio = 20190101;
            item1.dtFimBeneficio = 20191031;

            importador.itemMatriz = new List<ItemMatriz>();

            importador.itemMatriz.Add(item1);

            var item2 = new ItemMatriz();
            item2.cdNcmProdFinal = 12345678;
            item2.cdSuframa = 1234;
            item2.cdDestinacao = 12;
            item2.cdUtilizacao = 34;
            item2.cdTributacao = "ABCD";
            item2.numDecreto = 888882019;
            item2.dtInicioBeneficio = 20190101;
            item2.dtFimBeneficio = 20191031;

            importador.itemMatriz.Add(item2);
            #endregion

            #region gera Xml
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(importador.GetType());

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Importador));

            string path = Directory.GetCurrentDirectory() + @"\pli_xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString();

            var filePath = path + "\\" + fileName + ".xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, importador);
            file.Close();
            #endregion

            #region zipa arquivo
        
            using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(filePath, fileName);
            }

            return filePath;
            #endregion
        }
    }
}
