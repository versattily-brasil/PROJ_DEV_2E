using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Versattily.WebDriver;

namespace Versattily
{
    class Program
    {
        public static string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public static string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        public static string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";//?nrDeclaracao=19/0983204-0&consulta=true";
        //public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?";
        public static string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do";

        static void Main(string[] args)
        {

            using (var driver = new VersattilyDriver(FindClientCertificate("511d19041380bd8e")))
            {
                Console.WriteLine("Inciando processo de navegação...");

                string numero = "1909832040";
                //Navega para seguinda url.
                //página da consulta DI.
                //browser.Navigate(_urlConsultaDI);
                driver.Navigate().GoToUrl(_urlSite);
                driver.Navigate().GoToUrl(_urlConsultaDI);
                Console.WriteLine(_urlConsultaDI);

                //obtendo o campo de numero de declaração.
                Console.WriteLine("inserindo o numero da declaração");
                driver.FindElement(By.Id("nrDeclaracao")).SendKeys(numero);
                //browser.Find("nrDeclaracao").Value = numero;

                Console.WriteLine("Acionando o Click no enviar.");
                Thread.Sleep(1000);

                driver.FindElement(By.Name("enviar")).Click();

                //browser.Find(ElementType.Button, FindBy.Name, "enviar").Click();
                Thread.Sleep(1000);

                //indo para a página de consulta de declaração de importação.
                //browser.Find("btnRegistrarDI").Click();
                driver.FindElement(By.Id("btnRegistrarDI")).Click();

                Thread.Sleep(1000);

                Console.WriteLine("Baixando o Extrato - XML.");
                Thread.Sleep(1000);

                var gerouXml = DownloadExtratoXML(driver, numero);


            }
        }

        protected static bool DownloadExtratoXML(VersattilyDriver browser, string numero)
        {
            try
            {
                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.xml");


                if (!File.Exists(arquivoPath))
                {
                    browser._my.Navigate(_urlConsultaDI);
                    browser._my.Navigate(new Uri(_urlDownloadXML),
                        "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + numero + "&numeroRetificacao=&enviar=Consultar",
                        "application/x-www-form-urlencoded");

                    browser.FindElement(By.Id("nrDeclaracaoXml")).SendKeys(numero);
                    browser.FindElement(By.Name("ConsultarDiXmlForm")).Submit();

                    Thread.Sleep(5000);

                    File.WriteAllBytes(arquivoPath, ConvertToByteArray(browser.PageSource));
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
            return arr;
        }

        public static X509Certificate FindClientCertificate(string serialNumber)
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
    }
}
