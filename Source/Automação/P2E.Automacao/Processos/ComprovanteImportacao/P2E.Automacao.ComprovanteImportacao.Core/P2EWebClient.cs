using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Versattily.WebDriver;

namespace P2E.Automacao.Processos.ComprovanteImportacao.Core
{
    public class P2EWebClient : WebClient
    {
        private readonly X509Certificate certificate;
        private readonly VersattilyDriver driver;

        public P2EWebClient(X509Certificate cert, VersattilyDriver driver)
        {
            certificate = cert;
            this.driver = driver;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)base.GetWebRequest(address);

            httpRequest.ClientCertificates.Add(certificate);

            httpRequest.CookieContainer = new CookieContainer();

            foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
            {
                System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

                httpRequest.CookieContainer.Add(cookie);
            }

            //httpRequest.Timeout = 5000;

            return httpRequest;
        }
    }
}
