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

                    var teste = _driver.FileDetector;

                   bool result = DownloadExtrato(_driver);

                    if (result)
                    {
                        Console.WriteLine("Download completo");
                    }
                    else
                    {
                        Console.WriteLine("Erro!");
                    }

                    Console.WriteLine(_driver.PageSource);

                    Console.ReadKey();
                }
            }
        }

        protected void Importar()
        {
            
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

                var certificado = new X509Certificate();

                string diretorioCert = ControleCertificados.CertificadoPath();

                string path = Path.Combine(diretorioCert, "client - certificate");

                request.ClientCertificates.Add(new X509Certificate(path, "pass"));

                // download file
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = File.Create("C:\\Users\\Jorge.PATRIMONIO\\Desktop\\arquivo\\ExtratoTeste"))
                {
                    var buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: "+e.Message);
                return false;
            }
        }
    }
}
