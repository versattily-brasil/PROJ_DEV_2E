using Ionic.Zip;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Processos.EnviarPLI.Lib.Entidades;
using P2E.Automacao.Processos.EnviarPLI.Lib.Entidades.EstruturaPLI;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        public void Executar(object o)
        {
            //Thread.Sleep(10000);
            // Obter processos não registrados

           
            // Gerar Arquivo PLI ?

            // Preparar Arquivo PLI para envio

            // Enviar arquivo ZIP
            EnviarArquivo();

            // Recuperar Erros ocorrerem

            // Registrar Execução

            // Registrar Erros
        }

        private void EnviarArquivo()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService())
            {
                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("login");

                    var credencial = ObterCredenciaisSuframa();

                    element.SendKeys(credencial.Usuario);

                    element = _driver.FindElementByName("field(-senha)");

                    element.SendKeys(credencial.Senha);

                    element = _driver.FindElementByName("btLogar");
                    element.Click();

                    // localizar ~link com o texto "Enviar PLI e efetuar o click"
                    element = _driver.FindElementByPartialLinkText("Enviar PLI");
                    element.Click();

                    // localizar o elemento File com o nome manter-arquivo
                    element = _driver.FindElementByName("field(-manter-arquivo)");

                    // seleciona o arquivo
                    element.SendKeys(GerarArquivoPLI());

                    //localiza e clica no botão Enviar PLI
                    element = _driver.FindElementById("btnEnviarpli");
                    element.Click();

                    // Recupera a mensagem de erro caso exista.
                    try
                    {
                        element = _driver.FindElementById("ERROR");
                    }
                    catch (Exception){}

                    Console.WriteLine(element.Text);
                }
            }
        }

        private string GerarArquivoPLI()
        {
            #region Obtem dados da PLI  -  Dados Mockados
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

            string path = Path.GetTempPath() + "//xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString();

            var filePath = path + "\\" + fileName + ".xml";
            using (StreamWriter file = File.AppendText(filePath))
            {
                writer.Serialize(file, importador);
                file.Close();
            }
            #endregion

            #region zipa arquivo
            var files = Directory.GetFiles(path, fileName + ".xml");

            //provide the path and name for the zip file to create
            string zipFile = path + "\\" + fileName + ".PL5ZIP";

            CriarArquivoZip(files.ToList(), zipFile);

            return zipFile;
            #endregion
        }

        private static void CriarArquivoZip(List<string> arquivos, string ArquivoDestino)
        {
            using (ZipFile zip = new ZipFile())
            {
                // percorre todos os arquivos da lista
                foreach (string item in arquivos)
                {
                    // se o item é um arquivo
                    if (File.Exists(item))
                    {
                        try
                        {
                            // Adiciona o arquivo na pasta raiz dentro do arquivo zip
                            zip.AddFile(item, "");
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    // se o item é uma pasta
                    else if (Directory.Exists(item))
                    {
                        try
                        {
                            // Adiciona a pasta no arquivo zip com o nome da pasta 
                            zip.AddDirectory(item, new DirectoryInfo(item).Name);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                // Salva o arquivo zip para o destino
                try
                {
                    zip.Save(ArquivoDestino);
                }
                catch
                {
                    throw;
                }
            }
        }

        private CredenciaisSuframa ObterCredenciaisSuframa()
        {
            // Obter credenciais para acesso ao site
            return new CredenciaisSuframa() { Usuario = "08281892000158", Senha = "2edespachos" };
        }
    }
}
