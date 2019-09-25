using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.BaixarExtrato.Lib
{
    public class P2EWebClient : WebClient
    {
        private readonly X509Certificate certificate;
        private readonly PhantomJSDriver driver;

        public P2EWebClient(X509Certificate cert, PhantomJSDriver driver)
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
