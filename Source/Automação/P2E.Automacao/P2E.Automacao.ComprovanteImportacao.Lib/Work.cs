using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using System.IO;

namespace P2E.Automacao.ComprovanteImportacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        #endregion
        public void Executar(object o)
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
                    _driver.Navigate().GoToUrl(_urlSite);
                }
            }
        }
    }
}
