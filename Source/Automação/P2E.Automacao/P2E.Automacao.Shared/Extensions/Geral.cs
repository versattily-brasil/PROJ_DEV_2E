using SimpleBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Shared.Extensions
{
    public static class Geral
    {
        public static Browser CriarBrowser(string idCertificado)
        {
            Console.WriteLine("CARREGANDO O CERTIFICADO...");
            var certificado = ControleCertificados.FindClientCertificate(idCertificado);
            var browser = new Browser(certificado);
            browser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";

            return browser;
        }
    }
}
