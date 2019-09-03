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
using P2E.Automacao.AcompanharDespachos.Lib.Entities;

namespace P2E.Automacao.AcompanharDespachos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        string urlAcompanhaDespacho = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/AcompanharSituacaoDespachoMenu.do";
        string corStatus = string.Empty;
        string data = string.Empty;
        #endregion

        public async void Executar()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    TBImportacao tbImportacao = null;
                    Historico historicoImp = new Historico();

                    client.BaseAddress = new Uri("http://localhost:7000/");
                    var result = client.GetAsync($"imp/v1/importacao/todos").Result;
                    result.EnsureSuccessStatusCode();

                    if (result.IsSuccessStatusCode)
                    {
                        var aux = await result.Content.ReadAsStringAsync();
                        var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

                        foreach (var item in importacao)
                        {
                            if (item.TX_NUM_DEC.Trim().Length == 10)
                            {
                                Acessar(item, item.TX_NUM_DEC, item.CD_IMP.ToString(), historicoImp);
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

        public static async Task AtualizaStatus(TBImportacao import, string cd_imp, Historico historico)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:7000/");

                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);

                    resultado.EnsureSuccessStatusCode();
                }

                if (resultado.IsSuccessStatusCode)
                {
                    using (var clientH = new HttpClient())
                    {
                        clientH.BaseAddress = new Uri("http://localhost:7000/");

                        var resultadoHist = await clientH.PutAsJsonAsync($"imp/v1/historico/{0}", historico);

                        resultadoHist.EnsureSuccessStatusCode();

                        if (resultadoHist.IsSuccessStatusCode)
                        {

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Acessar(TBImportacao import, string numero, string cd_imp, Historico historicoImp)
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
                            corStatus = element.Text;
                            import.TX_CANAL = corStatus;

                            switch (corStatus)
                            {
                                case "Verde":

                                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                                    data = element.Text;
                                    import.DT_DATA_DES = Convert.ToDateTime(data);

                                    break;

                                case "Vermelho":

                                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                                    data = element.Text;
                                    import.DT_DATA_DES = Convert.ToDateTime(data);

                                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(5) > td:nth-child(2)");
                                    var fiscal = element.Text;
                                    import.TX_NOME_FISCAL = fiscal;

                                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(1)");
                                    var dossie = element.Text;
                                    import.TX_DOSSIE = dossie;

                                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(2)");
                                    var dataDossie = element.Text;
                                    import.DT_DATA_DOSS = Convert.ToDateTime(dataDossie);

                                    break;

                                default:
                                    break;
                            }

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "EM ANALISE FISCAL":

                            //Cor do Canal
                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(3) > td:nth-child(3)");
                            corStatus = element.Text;
                            import.TX_CANAL = corStatus;

                            switch (corStatus)
                            {
                                case "Verde":

                                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(2)");
                                    var data = element.Text;
                                    import.DT_DATA_DISTR = Convert.ToDateTime(data);



                                    break;

                                case "Vermelho":
                                    break;

                                default:
                                    break;
                            }

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "SELECIONADA PARA CONFERENCIA PELA ADUANA":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "EM DESEMBARACO":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "AGUARDANDO DISTRIBUICAO":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "EM DISTRIBUICAO":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "AGUARDANDO RECEPCAO DOS DOCUMENTOS":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "DECLARACAO EM ANALISE":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        case "DESPACHO INTERROMPIDO":

                            import.TX_STATUS = status;

                            SalvaHistoricoImp(numDeclaracao, historicoImp, status);

                            break;

                        default:
                            break;
                    }

                    AtualizaStatus(import, cd_imp, historicoImp);

                    Console.WriteLine(import.ToString());
                }
            }
        }

        private void SalvaHistoricoImp(string numDeclaracao, Historico historicoImp, string status)
        {
            historicoImp.TX_NUM_DEC = numDeclaracao;
            historicoImp.TX_STATUS = status;
            historicoImp.TX_CANAL = corStatus;
            historicoImp.DT_DATA = DateTime.Now.Date;
            historicoImp.HR_HORA = DateTime.Now;
        }
    }
}
