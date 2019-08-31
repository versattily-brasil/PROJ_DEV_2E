using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace P2E.Automacao.AcompanharDespachos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        string urlAcompanhaDespacho = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/AcompanharSituacaoDespachoMenu.do";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;

        private string _urlImportacao;
        private readonly AppSettings appSettings;

        #endregion
        public void Executar()
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:7000/");
                    var result = client.GetAsync($"imp/v1/tbimportacao/todos").Result;
                    //var result = client.GetAsync($"sso/v1/grupo/{5}").Result;
                    result.EnsureSuccessStatusCode();


                    if (result.IsSuccessStatusCode)
                    {
                      
                    }
                                       
                }

            }
            catch (Exception e)
            {
                throw;
            }



            Acessar();
        }

        protected void Acessar()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                var numDeclaracao = "1915359807";

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);
                    _driver.Navigate().GoToUrl(urlAcompanhaDespacho);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
                    element.SendKeys(numDeclaracao);

                    // clica no link contendo o 'Consultar'
                    element = _driver.FindElementByCssSelector(@"#botoes > input:nth-child(1)");
                    element.Click();

                    string Numero = numDeclaracao.Substring(0, 2) +"/" + 
                                    numDeclaracao.Substring(2, 7) + "-"+
                                    numDeclaracao.Substring(9, 1);

                    // clica no link contendo o ...
                    element = _driver.FindElementByPartialLinkText(Numero);
                    element.Click();

                    //localiza o status do despacho
                    element = _driver.FindElementByCssSelector("#tr_"+ numDeclaracao + " > td:nth-child(2)");
                    var status = element.Text;

                    switch (status)
                    {
                        case "DECLARACAO DESEMBARACADA":

                            //Cor do Canal
                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                            var Cor = element.Text;

                            //Data do Desembaraço
                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                            var data = element.Text;
                                                       
                            break;

                        case "EM ANALISE FISCAL":
                            break;

                        case "SELECIONADA PARA CONFERENCIA PELA ADUANA":
                            break;

                        case "EM DESEMBARACO":
                            break;

                        case "AGUARDANDO DISTRIBUICAO":
                            break;

                        case "EM DISTRIBUICAO":
                            break;

                        case "AGUARDANDO RECEPCAO DOS DOCUMENTOS":
                            break;

                        case "DECLARACAO EM ANALISE":
                            break;

                        case "DESPACHO INTERROMPIDO":
                            break;

                        default:
                            break;
                    }

                    Console.WriteLine(element.Text);
                }
            }
        }
    }
}
