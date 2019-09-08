using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace P2E.Automacao.Shared.Extensions
{
    public static class ControleCertificados
    {
        public static void CarregarCertificado(PhantomJSDriverService service)
        {
            Console.WriteLine("Carregando Certificado.");
            service.IgnoreSslErrors = true;
            string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.crt";
            service.AddArgument(cert);
            service.AddArgument($"--ssl-client-key-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.key");
            service.AddArgument($"--ssl-client-key-passphrase=2e123456$");
        }
    }
}
