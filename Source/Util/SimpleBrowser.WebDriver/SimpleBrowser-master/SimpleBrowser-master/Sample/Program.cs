// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="SimpleBrowser">
// Copyright © 2010 - 2019, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace Sample
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using SimpleBrowser;

    internal class Program
    {
        public static string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        public string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";//?nrDeclaracao=19/0983204-0&consulta=true";
        //public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?";
        public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?nrDeclaracao=19%2F0983204-0&consulta=true";

        private static void Main(string[] args)
        {
            BaixarXml("D:\\Comandos\\julio.xml", "1909832040");
            //try
            //{
            //    // log the browser request/response data to files so we can interrogate them in case of an issue with our scraping
            //    browser.RequestLogged += OnBrowserRequestLogged;
            //    browser.MessageLogged += new Action<Browser, string>(OnBrowserMessageLogged);

            //    // we'll fake the user agent for websites that alter their content for unrecognised browsers
            //    browser.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10";

            //    // browse to GitHub
            //    browser.Navigate(_urlSite);
            //    if (LastRequestFailed(browser))
            //    {
            //        // always check the last request in case the page failed to load
            //        return;
            //    }

            //    // click the login link and click it
            //    browser.Log("First we need to log in, so browse to the login page, fill in the login details and submit the form.");
            //    HtmlResult loginLink = browser.Find("a", FindBy.Text, "Sign&nbsp;in");
            //    if (!loginLink.Exists)
            //    {
            //        browser.Log("Can't find the login link! Perhaps the site is down for maintenance?");
            //    }
            //    else
            //    {
            //        loginLink.Click();
            //        if (LastRequestFailed(browser))
            //        {
            //            return;
            //        }

            //        // fill in the form and click the login button - the fields are easy to locate because they have ID attributes
            //        browser.Find("login_field").Value = "youremail@domain.com";
            //        browser.Find("password").Value = "yourpassword";
            //        browser.Find(ElementType.Button, "name", "commit").Click();
            //        if (LastRequestFailed(browser))
            //        {
            //            return;
            //        }

            //        // see if the login succeeded - ContainsText() is very forgiving, so don't worry about whitespace, casing, html tags separating the text, etc.
            //        if (browser.ContainsText("Incorrect username or password"))
            //        {
            //            browser.Log("Login failed!", LogMessageType.Error);
            //        }
            //        else
            //        {
            //            // After logging in, we should check that the page contains elements that we recognise
            //            if (!browser.ContainsText("Your Repositories"))
            //            {
            //                browser.Log("There wasn't the usual login failure message, but the text we normally expect isn't present on the page");
            //            }
            //            else
            //            {
            //                browser.Log("Your News Feed:");
            //                // we can use simple jquery selectors, though advanced selectors are yet to be implemented
            //                foreach (HtmlResult item in browser.Select("div.news .title"))
            //                {
            //                    browser.Log("* " + item.Value);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    browser.Log(ex.Message, LogMessageType.Error);
            //    browser.Log(ex.StackTrace, LogMessageType.StackTrace);
            //}
            //finally
            //{
            //    string path = WriteFile("log-" + DateTime.UtcNow.Ticks + ".html", browser.RenderHtmlLogFile("SimpleBrowser Sample - Request Log"));

            //    Console.WriteLine("Log file published to:");
            //    Console.WriteLine(path);

            //    var process = new Process();
            //    process.StartInfo.FileName = path;
            //    process.StartInfo.UseShellExecute = true;
            //    process.Start();
            //}
        }

        public static bool BaixarXml(string sNomeArquivo, string NrDi)
        {
            try
            {
                sNomeArquivo = sNomeArquivo.Replace("/", "_");
                string sSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
                Uri sUri = new Uri(sSite);
                NrDi = NrDi.Replace("/", "");
                NrDi = NrDi.Replace("-", "");
                var certificado = FindClientCertificate("511d1904137f8ed4");
                Browser browser = new Browser(certificado);

                browser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                //browser.KeepAlive = true;

                browser.Navigate(new Uri("https://www1.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/login_cert.jsp"));
                browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do"));


                // PEGA NOME DO CLIENTE
                string sDiAlterada = string.Empty;

                if (!File.Exists(sNomeArquivo))
                {
                    browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do"));

                    // pdf extrato
                    browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDI.do"),
                        "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + NrDi + "&numeroRetificacao=&enviar=Consultar",
                        "application/x-www-form-urlencoded");

                    browser.Find("input", FindBy.Id, "nrDeclaracaoXml").Value = NrDi;
                    browser.Find("form", FindBy.Name, "ConsultarDiXmlForm").SubmitForm();

                    for (Int32 jj = 0; jj <= 8; jj++)
                    {
                        Thread.Sleep(200);
                    }

                    File.WriteAllBytes(sNomeArquivo, ConvertToByteArray(browser.CurrentHtml));

                }
                browser.Close();
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw;
            }

        }

        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);

            return arr;
        }
        private static bool LastRequestFailed(Browser browser)
        {
            if (browser.LastWebException != null)
            {
                browser.Log("There was an error loading the page: " + browser.LastWebException.Message);
                return true;
            }
            return false;
        }

        private static void OnBrowserMessageLogged(Browser browser, string log)
        {
            Console.WriteLine(log);
        }

        private static void OnBrowserRequestLogged(Browser req, HttpRequestLog log)
        {
            Console.WriteLine(" -> " + log.Method + " request to " + log.Url);
            Console.WriteLine(" <- Response status code: " + log.ResponseCode);
        }

        private static string WriteFile(string filename, string text)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
            if (!dir.Exists)
            {
                dir.Create();
            }

            string path = Path.Combine(dir.FullName, filename);
            File.WriteAllText(path, text);
            return path;
        }


        public static X509Certificate FindClientCertificate(string serialNumber)
        {
            var store = new X509Store("MY", StoreLocation.CurrentUser);

            var Key = new RSACryptoServiceProvider();
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = store.Certificates;
            X509Certificate2Collection fcollection = collection.Find(X509FindType.FindBySerialNumber, serialNumber, false);
            if (fcollection.Count == 1)
            {
                return fcollection[0];
            }
            else
            {
                //Aqui pode colocar para escolher manualmente o certificado.
            }
            return null;
        }
    }
}