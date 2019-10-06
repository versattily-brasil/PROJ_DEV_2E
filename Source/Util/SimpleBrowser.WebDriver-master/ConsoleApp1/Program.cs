using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
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

            //using (var driver = new VersattilyDriver(FindClientCertificate("511d1904137f8ed4")))
            using(var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(FindClientCertificate("511d1904137f8ed4")))
            {
                Console.WriteLine("Inciando processo de navegação...");

                string numero = "1909832040";
               

                var gerouXml = DownloadExtratoXML(driver, numero);


            }
        }

        protected static bool DownloadExtratoXML(SimpleBrowser.WebDriver.SimpleBrowserDriver browser, string numero)
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
                    browser._my.Navigate(_urlSite);
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
