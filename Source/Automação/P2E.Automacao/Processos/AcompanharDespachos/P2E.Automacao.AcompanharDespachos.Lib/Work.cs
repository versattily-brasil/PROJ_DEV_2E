using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.AcompanharDespachos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string urlAcompanhaDespacho = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/AcompanharSituacaoDespachoMenu.do";
        public string corStatus = string.Empty;
        public string data = string.Empty;
        public string fiscal = string.Empty;
        public string dossie, dataDossie = string.Empty;
        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#####################  INICIALIZANDO - ACOMPANHAMENTO DE DESPACHO  ##################### ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para exoneração.");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/todos";

            using (var client = new HttpClient())
            {
                Historico historicoImp = new Historico();
                Vistoria vistoriaImp = new Vistoria();

                Console.WriteLine("ABRINDO CONEXAO...");
                var result = await client.GetAsync(urlAcompanha);
                registros = await result.Content.ReadAsAsync<List<Importacao>>();

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService())
                    {
                        Console.WriteLine("CARREGANDO O CERTIFICADO...");
                        ControleCertificados.CarregarCertificado(service);

                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            try
                            {
                                //ACESSANDO PAGINA PRINCIPAL
                                _driver.Navigate().GoToUrl(_urlSite);

                                foreach (var di in registros)
                                {
                                    //FILTRANDO O STATUS DA DI. TAMANHO 10/ CANAL VERDE == 1 / DESEMBARAÇADA == 11
                                    if (di.TX_NUM_DEC.Trim().Length == 10 && di.CD_IMP_CANAL != 1 && di.CD_IMP_STATUS != 11)
                                    {
                                        Console.WriteLine("################## DI: " + di.TX_NUM_DEC + " ##################");

                                        List<Thread> threads = new List<Thread>();

                                        var thread = new Thread(() => Acessar(di, di.TX_NUM_DEC, di.CD_IMP.ToString(), historicoImp, vistoriaImp, _driver));
                                        thread.Start();
                                        threads.Add(thread);

                                        // fica aguardnado todas as threads terminarem...
                                        while (threads.Any(t => t.IsAlive))
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                _driver.Close();
                            }

                        }

                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                }
            }
        }

        private async Task Acessar(Importacao import, string numero, string cd_imp, Historico historicoImp, Vistoria vistoriaImp, PhantomJSDriver _driver)
        {
            var numDeclaracao = numero;

            //PAGINA DE CONSULTA
            _driver.Navigate().GoToUrl(urlAcompanhaDespacho);

            OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
            element.SendKeys(numDeclaracao);

            // clica no link contendo o 'Consultar'
            element = _driver.FindElementByCssSelector(@"#botoes > input:nth-child(1)");
            element.Click();

            string Numero = numDeclaracao.Substring(0, 2) + "/" +
                            numDeclaracao.Substring(2, 7) + "-" +
                            numDeclaracao.Substring(9, 1);

            try
            {
                // clica no link contendo o ...
                element = _driver.FindElementByPartialLinkText(Numero);
                element.Click();

            }
            catch (Exception) { }

            //localiza o status do despacho
            element = _driver.FindElementByCssSelector("#tr_" + numDeclaracao + " > td:nth-child(2)");
            var status = element.Text;
            Console.WriteLine("-----" + status + "-----");

            switch (status)
            {
                case "DECLARACAO DESEMBARACADA":

                    //Cor do Canal
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                    corStatus = element.Text;

                    switch (corStatus)
                    {
                        case "Verde":

                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                            data = element.Text;
                            import.DT_DATA_DES = Convert.ToDateTime(data);
                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                            data = element.Text;
                            import.DT_DATA_DES = Convert.ToDateTime(data);

                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(5) > td:nth-child(2)");
                            fiscal = element.Text;
                            import.TX_NOME_FISCAL = fiscal;

                            element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(1)");
                            var dossie = element.Text;
                            import.TX_DOSSIE = dossie;

                            element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(2)");
                            var dataDossie = element.Text;
                            import.DT_DATA_DOSS = Convert.ToDateTime(dataDossie);

                            import.CD_IMP_CANAL = 3;

                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 11;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "EM ANALISE FISCAL":

                    //Cor do Canal
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(3) > td:nth-child(3)");
                    corStatus = element.Text;

                    switch (corStatus)
                    {
                        case "Verde":

                            element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(2)");
                            data = element.Text;
                            import.DT_DATA_DISTR = Convert.ToDateTime(data);
                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 1;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "SELECIONADA PARA CONFERENCIA PELA ADUANA":

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 3;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "DI AGUARDANDO PARAMETRIZACAO":

                    import.CD_IMP_CANAL = 0;

                    import.CD_IMP_STATUS = 2;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "AGUARDANDO DISTRIBUICAO":

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 5;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "EM DISTRIBUICAO":

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 7;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "DI AGUARDANDO RECEPCAO DE DOCUMENTOS":

                    //Cor do Canal
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                    corStatus = element.Text;

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 8;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "DECLARACAO EM ANALISE":

                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(7) > td:nth-child(2)");
                    data = element.Text;
                    import.DT_DATA_DISTR = Convert.ToDateTime(data);

                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                    fiscal = element.Text;
                    import.TX_NOME_FISCAL = fiscal;

                    //Cor do Canal
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                    corStatus = element.Text;

                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(1)");
                    dossie = element.Text;
                    import.TX_DOSSIE = dossie;

                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(2)");
                    dataDossie = element.Text;
                    import.DT_DATA_DOSS = Convert.ToDateTime(dataDossie);

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }

                    import.CD_IMP_STATUS = 9;
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                    break;

                case "DESPACHO INTERROMPIDO":

                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                    fiscal = element.Text;
                    import.TX_NOME_FISCAL = fiscal;

                    //Cor do Canal
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                    corStatus = element.Text;

                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(1)");
                    dossie = element.Text;
                    import.TX_DOSSIE = dossie;

                    element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(2)");
                    dataDossie = element.Text;
                    import.DT_DATA_DOSS = Convert.ToDateTime(dataDossie);

                    // clica no link contendo o ...
                    element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(8) > td:nth-child(5) > a > img");
                    element.Click();

                    //ESCOLHE A NOVA JANELA ABERTA COM O CLIQUE
                    try
                    {
                        _driver.SwitchTo().Window(_driver.WindowHandles[2]);
                        element = _driver.FindElementByCssSelector("#box > div > div > textarea");
                    }
                    catch (Exception e)
                    {
                        _driver.SwitchTo().Window(_driver.WindowHandles[1]);
                        element = _driver.FindElementByXPath("//*[@id='box']/div/div/textarea");
                    }

                    var motivo = element.Text;

                    //salva vistoria
                    vistoriaImp.CD_IMP = import.CD_IMP;
                    vistoriaImp.TX_DESC = motivo.Trim();

                    import.CD_IMP_STATUS = 10;

                    switch (corStatus)
                    {
                        case "Verde":

                            import.CD_IMP_CANAL = 1;

                            break;

                        case "Vermelho":

                            import.CD_IMP_CANAL = 3;
                            break;

                        default:
                            break;
                    }
                    Console.WriteLine("ATUALIZANDO HISTORICO...");
                    await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);
                    Console.WriteLine("ATUALIZANDO VISTORIA...");
                    await AtualizaVistoria(cd_imp, vistoriaImp);

                    break;

                default:
                    break;
            }
            Console.WriteLine("ATUALIZANDO STATUS...");
            await AtualizaStatus(import, cd_imp, historicoImp);

        }

        private async Task AtualizaStatus(Importacao import, string cd_imp, Historico historico)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);

                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);

                    resultado.EnsureSuccessStatusCode();
                }

                if (resultado.IsSuccessStatusCode)
                {
                    using (var clientH = new HttpClient())
                    {
                        clientH.BaseAddress = new Uri(_urlApiBase);

                        var resultadoHist = await clientH.PutAsJsonAsync($"imp/v1/historico/{0}", historico);

                        resultadoHist.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private async Task AtualizaVistoria(string cd_imp, Vistoria vistoriaImp)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    var result = client.GetAsync($"imp/v1/vistoria/{cd_imp}").Result;
                    result.EnsureSuccessStatusCode();

                    if (result.IsSuccessStatusCode)
                    {
                        using (var clientH = new HttpClient())
                        {
                            clientH.BaseAddress = new Uri(_urlApiBase);

                            var imp = 0;
                            if (result.ReasonPhrase == "No Content")
                            {
                                imp = 0;
                            }
                            else
                            {
                                var resultadoDelete = await clientH.DeleteAsync($"imp/v1/vistoria/{cd_imp}");
                                resultadoDelete.EnsureSuccessStatusCode();
                            }

                            var resultadoHist = await clientH.PutAsJsonAsync($"imp/v1/vistoria/{imp}", vistoriaImp);

                            resultadoHist.EnsureSuccessStatusCode();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private async Task SalvaHistoricoImp(string numDeclaracao, Historico historicoImp, int cd_imp, int cd_imp_status, int cd_imp_canal)
        {
            historicoImp.CD_IMP = cd_imp;
            historicoImp.CD_IMP_STATUS = cd_imp_status;
            historicoImp.CD_IMP_CANAL = cd_imp_canal;
            historicoImp.TX_NUM_DEC = numDeclaracao;
            historicoImp.DT_DATA = DateTime.Now.Date;
            historicoImp.HR_HORA = DateTime.Now;
        }




        //public async void CarregarImportacao()
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            TBImportacao tbImportacao = null;
        //            Historico historicoImp = new Historico();
        //            Vistoria vistoriaImp = new Vistoria();

        //            Console.WriteLine("ABRINDO CONEXAO...");
        //            client.BaseAddress = new Uri("http://localhost:7000/");
        //            var result = client.GetAsync($"imp/v1/importacao/todos").Result;
        //            result.EnsureSuccessStatusCode();

        //            if (result.IsSuccessStatusCode)
        //            {
        //                var aux = await result.Content.ReadAsStringAsync();
        //                var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

        //                foreach (var item in importacao)
        //                {
        //                    if (item.TX_NUM_DEC.Trim().Length == 10 && item.CD_IMP_CANAL != 1 && item.CD_IMP_STATUS != 11)
        //                    {


        //                        Acessar(item, item.TX_NUM_DEC, item.CD_IMP.ToString(), historicoImp, vistoriaImp);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Número da declaração está incorreto! " + e.ToString());
        //    }
        //}
    }
}
