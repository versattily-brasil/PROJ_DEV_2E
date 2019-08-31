using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Xml;

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

        private List<TBImportacao> _tbImportacao;

        #endregion

        public class TBImportacao
        {
            public int CD_IMP { get; set; }
            public int NUM_PI { get; set; }
            public int NR_NUM_DEC { get; set; }
            public string TX_STATUS { get; set; }
            public string TX_CANAL { get; set; }
            public DateTime DT_DATA_DES { get; set; }
            public decimal VL_MULTA { get; set; }
            public string TX_NOME_FISCAL { get; set; }
            public DateTime DT_DATA_CANAL { get; set; }
            public DateTime DT_DATA_DISTR { get; set; }
        }

        public async void Executar()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    TBImportacao tbImportacao = null;

                    XmlDocument doc = new XmlDocument();

                    client.BaseAddress = new Uri("http://localhost:7000/");
                    var result = client.GetAsync($"imp/v1/tbimportacao/todos").Result;
                    result.EnsureSuccessStatusCode();

                    if (result.IsSuccessStatusCode)
                    {
                        var aux = await result.Content.ReadAsStringAsync();
                        var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

                        foreach (var item in importacao)
                        {
                            if (item.NR_NUM_DEC.ToString().Length == 10)
                            {
                                Acessar(item, item.NR_NUM_DEC.ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static async Task AtualizaStatus (TBImportacao import, string cd_imp)
        {
            var resultado = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:7000/");

                    resultado = client.PutAsJsonAsync($"imp/v1/tbimportacao/todos/{cd_imp}", import);
                    responseBody = await resultado.Content.ReadAsStringAsync();
                    resultado.EnsureSuccessStatusCode();



                    //client.BaseAddress = new Uri("http://localhost:7000/");
                    //var result = client.GetAsync($"imp/v1/tbimportacao/{cd_imp}").Result;
                    //result.EnsureSuccessStatusCode();

                    //if (result.IsSuccessStatusCode)
                    //{
                    //    var aux = await result.Content.ReadAsStringAsync();
                    //    var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);


                    //}
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        protected void Acessar(TBImportacao import, string numero)
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                var numDeclaracao = numero;

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);
                    _driver.Navigate().GoToUrl(urlAcompanhaDespacho);

                    OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
                    element.SendKeys(numDeclaracao);

                    // clica no link contendo o 'Consultar'
                    element = _driver.FindElementByCssSelector(@"#botoes > input:nth-child(1)");
                    element.Click();

                    string Numero = numDeclaracao.Substring(0, 2) + "/" +
                                    numDeclaracao.Substring(2, 7) + "-" +
                                    numDeclaracao.Substring(9, 1);

                    // clica no link contendo o ...
                    element = _driver.FindElementByPartialLinkText(Numero);
                    element.Click();

                    //localiza o status do despacho
                    element = _driver.FindElementByCssSelector("#tr_" + numDeclaracao + " > td:nth-child(2)");
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
