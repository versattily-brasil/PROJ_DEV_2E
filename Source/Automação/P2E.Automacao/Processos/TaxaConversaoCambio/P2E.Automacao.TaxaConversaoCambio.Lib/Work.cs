using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.TaxaConversaoCambio.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www35.receita.fazenda.gov.br/tabaduaneiras-web/public/pages/security/login_publico.jsf";

        private string _urlApiBase;
        private List<Importacao> registros;
        private string arquivoPath = "";
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - TAXA DE CONVERSÃO DE CAMBIO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            using (var client = new HttpClient())
            {
                using (var service = PhantomJSDriverService.CreateDefaultService())
                {
                    service.AddArgument("test-type");
                    service.AddArgument("no-sandbox");
                    service.HideCommandPromptWindow = true;

                    using (var _driver = new PhantomJSDriver(service))
                    {
                        try
                        {
                            Acessar(_driver);



                            Console.ReadKey();
                        }
                        catch (Exception)
                        {
                            _driver.Close();
                        }
                    }
                }

            }
        }

        private async Task Acessar(PhantomJSDriver _driver)
        {
            Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
            _driver.Navigate().GoToUrl(_urlSite);


        }

    }
}
