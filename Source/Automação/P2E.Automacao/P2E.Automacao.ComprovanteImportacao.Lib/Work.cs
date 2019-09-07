using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.ComprovanteImportacao.Lib.Entities;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace P2E.Automacao.ComprovanteImportacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        private string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/RecuperarComprovanteMenu.do";
        private string _urlImprimir = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/ImprimirComprovante.do";

        #endregion
        public void Executar(object o)
        {
            CarregarImportacao();
        }

        public async void CarregarImportacao()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    TBImportacao tbImportacao = null;

                    client.BaseAddress = new Uri("http://localhost:7000/");
                    var result = client.GetAsync($"imp/v1/importacao/todos").Result;
                    result.EnsureSuccessStatusCode();

                    if (result.IsSuccessStatusCode)
                    {
                        var aux = await result.Content.ReadAsStringAsync();
                        var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

                        foreach (var item in importacao)
                        {
                            if (item.TX_NUM_DEC.Trim().Length == 10)
                            {
                                Console.WriteLine("DI: " + item.TX_NUM_DEC);

                                Acessar(item, item.TX_NUM_DEC);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Número da declaração está incorreto! " + e.ToString());
            }
        }

        protected void Acessar(TBImportacao import, string numero)
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                // carrega o cerificado.. retirar se não for necessário.
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                var numDeclaracao = numero;

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);
                    _driver.Navigate().GoToUrl(_urlTelaConsulta);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
                    element.SendKeys(numDeclaracao);

                    // clica no BOTAO 'Confirmar'
                    element = _driver.FindElementById("confirmar");

                    element.Click();

                    string Numero = numDeclaracao.Substring(0, 2) + "/" +
                                    numDeclaracao.Substring(2, 7) + "-" +
                                    numDeclaracao.Substring(9, 1);

                    string id_tr = "tr_" + Numero;

                    element = _driver.FindElementById(id_tr);

                    var status = element.Text;

                    if (status.Contains("COMPROVANTE JA EMITIDO. UTILIZAR EMISSAO SEGUNDA VIA"))
                    {
                        // clica no botao OK
                        element = _driver.FindElementByCssSelector(@"#botoes > input");
                        element.Click();

                        // clica no radiobutton 2º via
                        element = _driver.FindElementByCssSelector(@"#corpo > fieldset:nth-child(1) > div > input[type=radio]:nth-child(2)");
                        element.Click();

                        element = _driver.FindElementById("nrDeclaracao");
                        element.SendKeys(numDeclaracao);

                        // clica no BOTAO 'Confirmar'
                        element = _driver.FindElementById("confirmar");
                        element.Click();
                    }
                    else if (status.Contains("DECLARACAO NAO ESTA DESEMBARACADA."))
                    {
                        return;
                    }

                    id_tr = "tr_" + Numero;

                    element = _driver.FindElementById(id_tr);
                    status = element.Text;

                    if (status.Contains("COMPROVANTE RECUPERADO COM SUCESSO"))
                    {
                        DownloadComprovante(_driver, _urlImprimir + "?id=" + Numero);
                    }
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

        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Concluído.");
        }

        public static bool DownloadFile(string url, IWebDriver driver)
        {
            try
            {
                // Construct HTTP request to get the file
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.CookieContainer = new System.Net.CookieContainer();

                for (int i = 0; i < driver.Manage().Cookies.AllCookies.Count - 1; i++)
                {
                    System.Net.Cookie ck = new System.Net.Cookie(driver.Manage().Cookies.AllCookies[i].Name, driver.Manage().Cookies.AllCookies[i].Value, driver.Manage().Cookies.AllCookies[i].Path, driver.Manage().Cookies.AllCookies[i].Domain);
                    httpRequest.CookieContainer.Add(ck);
                }

                httpRequest.Accept = "text/html, application/xhtml+xml, */*";
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

                //HttpStatusCode responseStatus;

                // Get back the HTTP response for web server
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream httpResponseStream = httpResponse.GetResponseStream();

                // Define buffer and buffer size
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                // Read from response and write to file
                FileStream fileStream = File.Create("C:\\Versatilly\\COMPROVANTE_DI.pdf");
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

        protected bool DownloadComprovante(PhantomJSDriver driver, string _url)
        {
            try
            {
                var certificado = FindClientCertificate("511d19041380bd8e");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", "COMPROVANTE_DI.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
