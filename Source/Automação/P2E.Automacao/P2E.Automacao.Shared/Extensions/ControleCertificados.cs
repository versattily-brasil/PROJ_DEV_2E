using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace P2E.Automacao.Shared.Extensions
{
    public static class ControleCertificados
    {
        private static string subName1 = @"CN=EROSMILDESON RUIZ DE CASTRO:91701040263, OU=Certificado PF A1, OU=16994652000129, OU=AC SOLUTI Multipla, OU=AC SOLUTI, OU=Autoridade Certificadora Raiz Brasileira v2, O=ICP-Brasil, C=BR";
        //"CN=ENDERSON RUIZ DE CASTRO:67635466291, OU=Certificado PF A1, OU=16994652000129, OU=AC SOLUTI Multipla, OU=AC SOLUTI, OU=Autoridade Certificadora Raiz Brasileira v2, O=ICP-Brasil, C=BR";
        private static string subName2 = "CN=ENDERSON RUIZ DE CASTRO:67635466291, OU=Certificado PF A1, OU=16994652000129, OU=AC SOLUTI Multipla, OU=AC SOLUTI, OU=Autoridade Certificadora Raiz Brasileira v2, O=ICP-Brasil, C=BR";

        public static void CarregarCertificado(PhantomJSDriverService service)
        {
            service.IgnoreSslErrors = true;
            string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.crt";
            service.AddArgument(cert);
            service.AddArgument($"--ssl-client-key-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.key");
            service.AddArgument($"--ssl-client-key-passphrase=2e123456$");
        }

        //public static X509Certificate FindClientCertificate(string serialNumber)
        //{
        //    return
        //        FindCertificate(StoreLocation.CurrentUser) ??
        //        FindCertificate(StoreLocation.LocalMachine);
        //    X509Certificate FindCertificate(StoreLocation location)
        //    {
        //        X509Store store = new X509Store(location);
        //        store.Open(OpenFlags.OpenExistingOnly);
        //        var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
        //        return certs.OfType<X509Certificate>().FirstOrDefault();
        //    };
        //}



        public static X509Certificate GetClientCertificate()
        {
            return
                FindCertificate(StoreLocation.CurrentUser) ??
                FindCertificate(StoreLocation.LocalMachine);
            X509Certificate FindCertificate(StoreLocation location)
            {
                X509Store store = new X509Store(location);
                store.Open(OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subName1, true);

                if(certs == null || certs.Count == 0)
                    certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subName2, true);

                return certs.OfType<X509Certificate>().FirstOrDefault();
            };
        }
    }
}
