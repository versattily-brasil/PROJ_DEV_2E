using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace P2E.Automacao.BaixarExtratos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        private string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        private string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        private string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do?nrDeclaracao=19/0983204-0&consulta=true";
        private string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do";
        private string _pathDir = @"C:\Users\Jorge.PATRIMONIO\Desktop\arquivo";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        private string _nrDeclaracao = "1909832040";

        #endregion
        public void Executar()
        {
            Acessar();
        }

        protected void Acessar()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {

                // carrega o cerificado.. retirar se não for necessário.
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;


                using (var _driver = new PhantomJSDriver(service))
                {
                    Console.WriteLine("Inciando processo de navegação...");



                    //navega para primeira url.
                    //onde é realizado o login através do certificado.
                    _driver.Navigate().GoToUrl(_urlSite);
                    Console.WriteLine(_driver.Url);

                    //Navega para seguinda url.
                    //página da consulta DI.
                    _driver.Navigate().GoToUrl(_urlConsultaDI);
                    Console.WriteLine(_driver.Url);

                    //obtendo o campo de numero de declaração.
                    IWebElement element = _driver.FindElementById("nrDeclaracao");

                    //inserindo o numero da declaração.
                    element.SendKeys(_nrDeclaracao);

                    element = _driver.FindElement(By.Name("enviar"));

                    element.Click();

                    //indo para a página de consulta de declaração de importação.
                    //_driver.FindElement(By.Id("btnRegistrarDI")).Click();

                    //bool result = DownloadExtrato(_driver);

                    element = _driver.FindElement(By.Id("consultarXmlDi"));

                    //element.Click();

                    DownloadXml(_driver);

                    

                    Console.ReadKey();
                }
            }
        }

        public X509Certificate FindClientCertificate(string serialNumber)
        {
            return
                FindCertificate(StoreLocation.CurrentUser) ??
                FindCertificate(StoreLocation.LocalMachine);
            X509Certificate FindCertificate(StoreLocation location)
            {
                X509Store store = new X509Store(location);
                store.Open(OpenFlags.OpenExistingOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
                return certs.OfType<X509Certificate>().FirstOrDefault();
            };
        }

        /// <summary>
        /// Verifica se o diretório existe.
        /// </summary>
        protected void AvaliarDiretorio()
        {

        }

        /// <summary>
        /// Realiza o download o arquivo de Extrato.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        protected bool DownloadExtrato(PhantomJSDriver driver)
        {
            try
            {
                var certificado = FindClientCertificate("511d1904137f8ed4");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop", "ExtratoDI" + horaData + ".pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_urlDownloadPDF, arquivoPath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cria o XML da página.
        /// </summary>
        /// <param name="driver"></param>
        protected void DownloadXml(PhantomJSDriver driver)
        {
            try
            {
                var certificado = FindClientCertificate("511d1904137f8ed4");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop", "ExtratoXML" + horaData + ".xml");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFileAsync(new Uri(_urlDownloadXML), arquivoPath);

                    myWebClient.DownloadFileCompleted += MyWebClient_DownloadFileCompleted;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }

        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Concluído.");
        }
    }
}
