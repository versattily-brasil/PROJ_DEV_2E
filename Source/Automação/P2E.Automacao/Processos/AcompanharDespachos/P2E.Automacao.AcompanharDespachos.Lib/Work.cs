using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Xml;
using static P2E.Automacao.AcompanharDespachos.Lib.Entities.Importacao;

namespace P2E.Automacao.AcompanharDespachos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        string urlAcompanhaDespacho = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/AcompanharSituacaoDespachoMenu.do";
   
        #endregion

        public async void Executar()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    TBImportacao tbImportacao = null;

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
                                Acessar(item, item.NR_NUM_DEC.ToString(), item.CD_IMP.ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Número da declaração está incorreto! " + e.ToString());
            }
        }

        public static async Task AtualizaStatus(TBImportacao import, string cd_imp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:7000/");

                    var resultado = await client.PutAsJsonAsync($"imp/v1/tbimportacao/{cd_imp}", import);
                    var responseBody = await resultado.Content.ReadAsStringAsync();
                    resultado.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Acessar(TBImportacao import, string numero, string cd_imp)
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
                            import.TX_CANAL = Cor;

                            //Data do Desembaraço
                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                            var data = element.Text;
                            import.DT_DATA_DES = Convert.ToDateTime(data);

                            import.TX_STATUS = "DESEMBARACO";

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

                    AtualizaStatus(import, cd_imp);

                    Console.WriteLine(import.ToString());
                }
            }
        }
    }
}
