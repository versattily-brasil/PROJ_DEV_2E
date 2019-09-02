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

namespace P2E.Automacao.BaixarExtratos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        private string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        private string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        private string _urlDownloadExtrato = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do?nrDeclaracao=19/0983204-0&consulta=true";
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

                    //navega para primeira url.
                    //onde é realizado o login através do certificado.
                    _driver.Navigate().GoToUrl(_urlSite);
                    Console.WriteLine(_driver.Url);

                    //Navega para seguinda url.
                    //página da consulta DI.
                    _driver.Navigate().GoToUrl(_urlConsultaDI);
                    Console.WriteLine(_driver.Url);

                    //obtendo o campo de numero de declaração.
                    //IWebElement element = _driver.FindElement(By.Id("nrDeclaracao"));
                    IWebElement element = _driver.FindElementById("nrDeclaracao");

                    //inserindo o numero da declaração.
                    element.SendKeys(_nrDeclaracao);

                    element = _driver.FindElement(By.Name("enviar"));

                    element.Click();

                    //indo para a página de consulta de declaração de importação.
                    _driver.FindElement(By.Id("btnRegistrarDI")).Click();

                    DownloadExtrato3(_driver);

                    //bool result = DownloadExtrato2(_urlDownloadExtrato,_driver);

                    //if (result)
                    //{
                    //    Console.WriteLine("Download completo");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Erro!");
                    //}

                    Console.WriteLine(_driver.PageSource);

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

        protected bool DownloadExtrato(PhantomJSDriver driver)
        {
            try
            {
                // create HttpWebRequest
                Uri uri = new Uri(_urlDownloadExtrato);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                // insert cookies
                request.CookieContainer = new CookieContainer();

                foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

                    request.CookieContainer.Add(cookie);
                }

                var certificado = FindClientCertificate("511d1904137f8ed4");

                request.ClientCertificates.Add(certificado);

                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop\\arquivo", "ExtratoDI.pdf");

                // download file
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = File.Create(arquivoPath))
                {
                    var buffer = new byte[4096];
                    int bytesRead = 0;

                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                return false;
            }
        }

        protected bool DownloadExtrato2(string url, PhantomJSDriver driver)
        {
            try
            {
                // Construct HTTP request to get the file
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                
                httpRequest.CookieContainer = new CookieContainer();

                foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

                    httpRequest.CookieContainer.Add(cookie);
                }


                var certificado = FindClientCertificate("511d1904137f8ed4");

                httpRequest.ClientCertificates.Add(certificado);

                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop\\arquivo", "ExtratoDI.pdf");

                //HttpStatusCode responseStatus;

                // Get back the HTTP response for web server
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream httpResponseStream = httpResponse.GetResponseStream();

                // Define buffer and buffer size
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                // Read from response and write to file
                FileStream fileStream = File.Create(arquivoPath);

                var teste = httpResponseStream.Read(buffer, 0, bufferSize);

                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void DownloadExtrato3(PhantomJSDriver driver)
        {
            var certificado = FindClientCertificate("511d1904137f8ed4");

            string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop", "ExtratoDI.pdf");

            using (WebClient myWebClient = new P2EWebClient(certificado, driver))
            {
                myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
                // Download the Web resource and save it into the current filesystem folder.
                Thread.Sleep(4000);

                myWebClient.DownloadFile(_urlDownloadExtrato, arquivoPath);
            }
        }
    }
}
