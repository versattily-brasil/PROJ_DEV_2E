using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
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
        private List<TriagemBot> triagem;
        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;

        #endregion

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "#####################  INICIALIZANDO - ACOMPANHAMENTO DE DESPACHO  ##################### ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + "#####################  INICIALIZANDO - ACOMPANHAMENTO DE DESPACHO  ##################### ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            try
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para exoneração.", eTipoLog.INFO, _cd_bot_exec, "bot");
                await CarregarListaDIAsync();
                LogController.RegistrarLog(_nome_cliente + " - " + "Finalizando execução.", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em ExecutarAsync. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }

        private async Task CarregarListaDIAsync()
        {
            try
            {
                string urlTriagem = _urlApiBase + $"imp/v1/triagembot/despacho/" + _cd_par;

                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(urlTriagem);
                    triagem = await result.Content.ReadAsAsync<List<TriagemBot>>();
                }

                if (triagem.Count > 0)
                {
                    string urlAcompanha = _urlApiBase + $"imp/v1/importacao/despacho/" + _cd_par;

                    using (var client = new HttpClient())
                    {
                        Historico historicoImp = new Historico();
                        Vistoria vistoriaImp = new Vistoria();
                        TriagemBot triagemImp = new TriagemBot();

                        LogController.RegistrarLog(_nome_cliente + " - " + "ABRINDO CONEXÃO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        var result = await client.GetAsync(urlAcompanha);
                        registros = await result.Content.ReadAsAsync<List<Importacao>>();

                        if (registros != null && registros.Any())
                        {
                            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
                            {
                                LogController.RegistrarLog(_nome_cliente + " - " + "CARREGANDO CERTIFICADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                                string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.crt";
                                LogController.RegistrarLog(_nome_cliente + " - " + cert, eTipoLog.INFO, _cd_bot_exec, "bot");

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

                                        if (registros != null)
                                        {
                                            LogController.RegistrarLog(_nome_cliente + " - " + $"{triagem.Count} DI's localizadas.", eTipoLog.INFO, _cd_bot_exec, "bot");

                                            foreach (var drTri in triagem)
                                            {
                                                foreach (var di in registros)
                                                {
                                                    if (drTri.NR_DI == di.TX_NUM_DEC)
                                                    {
                                                        //FILTRANDO O STATUS DA DI. TAMANHO 10/ CANAL VERDE == 1 / DESEMBARAÇADA == 11
                                                        if (di.TX_NUM_DEC.Trim().Length == 10 && di.CD_IMP_CANAL != 1 && di.CD_IMP_STATUS != 11)
                                                        {
                                                            LogController.RegistrarLog(_nome_cliente + " - " + "################## DI: " + di.TX_NUM_DEC + " ##################", eTipoLog.INFO, _cd_bot_exec, "bot");

                                                            List<Thread> threads = new List<Thread>();

                                                            var thread = new Thread(() => Acessar(di, di.TX_NUM_DEC, di.CD_IMP.ToString(), historicoImp, vistoriaImp, _driver, drTri.CD_TRIAGEM.ToString(), drTri));
                                                            thread.Start();
                                                            threads.Add(thread);

                                                            // fica aguardnado todas as threads terminarem...
                                                            while (threads.Any(t => t.IsAlive))
                                                            {
                                                                continue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (di.TX_NUM_DEC.Trim().Length == 10 && di.CD_IMP_CANAL == 1)
                                                            {
                                                                LogController.RegistrarLog(_nome_cliente + " - " + $"DI {di.TX_NUM_DEC} está com status 'CANAL VERDE'", eTipoLog.ALERTA, _cd_bot_exec, "bot");
                                                            }
                                                            else if (di.CD_IMP_STATUS != 11)
                                                            {
                                                                LogController.RegistrarLog(_nome_cliente + " - " + $"DI {di.TX_NUM_DEC} está com status 'DESEMBARAÇADA'", eTipoLog.ALERTA, _cd_bot_exec, "bot");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogController.RegistrarLog(_nome_cliente + " - " + $"Não foram localizadas DI's.", eTipoLog.ALERTA, _cd_bot_exec, "bot");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogController.RegistrarLog(_nome_cliente + " - " + "ERRO: - " + ex.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");

                                        _driver.Close();
                                    }

                                }

                            }

                            ////Console.ReadKey();
                        }
                        else
                        {
                            LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's para Acompanhar Despacho.", eTipoLog.ALERTA, _cd_bot_exec, "bot");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em CarregarListaDIAsync. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }

        private async Task Acessar(Importacao import, 
                                    string numero, 
                                    string cd_imp, 
                                    Historico historicoImp, 
                                    Vistoria vistoriaImp, 
                                    PhantomJSDriver _driver, 
                                    string cd_triagem, 
                                    TriagemBot triagem)
        {
            var numDeclaracao = numero;

            try
            {
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
                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " Status - " + status, eTipoLog.INFO, _cd_bot_exec, "bot");

                switch (status)
                {
                    case "DECLARACAO DESEMBARACADA":

                        //Cor do Canal
                        element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(3)");
                        corStatus = element.Text;

                        LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " Cor Status - " + corStatus, eTipoLog.INFO, _cd_bot_exec, "bot");

                        switch (corStatus)
                        {
                            case "Verde":

                                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                                data = element.Text;

                                try
                                {
                                    import.DT_DATA_DES = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDateTime(data);
                                }
                                catch (Exception)
                                {
                                    import.DT_DATA_DES = null;
                                }

                                import.CD_IMP_CANAL = 1;

                                triagem.OP_ACOMP_DESP_IMP = 1;

                                AtualizaTriagem(triagem, cd_triagem);

                                break;

                            case "Vermelho":

                                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(6) > td:nth-child(2)");
                                data = element.Text;

                                try
                                {
                                    import.DT_DATA_DES = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    import.DT_DATA_DES = null;
                                }

                                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(5) > td:nth-child(2)");
                                fiscal = element.Text;
                                import.TX_NOME_FISCAL = fiscal;

                                element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(1)");
                                var dossie = element.Text;
                                import.TX_DOSSIE = dossie;

                                element = _driver.FindElementByCssSelector("#TABLE_3 > tbody > tr:nth-child(2) > td:nth-child(2)");

                                try
                                {
                                    import.DT_DATA_DOSS = DateTime.ParseExact(dataDossie, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    import.DT_DATA_DOSS = null;
                                }

                                import.CD_IMP_CANAL = 3;

                                break;

                            default:
                                break;
                        }

                        import.CD_IMP_STATUS = 11;
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                        break;

                    case "EM ANALISE FISCAL":

                        //Cor do Canal
                        element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(3) > td:nth-child(3)");
                        corStatus = element.Text;

                        LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " Cor Status - " + corStatus, eTipoLog.INFO, _cd_bot_exec, "bot");

                        switch (corStatus)
                        {
                            case "Verde":

                                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(4) > td:nth-child(2)");
                                data = element.Text;

                                try
                                {
                                    import.DT_DATA_DISTR = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    import.DT_DATA_DISTR = null;
                                }

                                import.CD_IMP_CANAL = 1;

                                break;

                            case "Vermelho":

                                import.CD_IMP_CANAL = 3;
                                break;

                            default:
                                break;
                        }

                        import.CD_IMP_STATUS = 1;
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                        break;

                    case "DI AGUARDANDO PARAMETRIZACAO":

                        import.CD_IMP_CANAL = 0;

                        import.CD_IMP_STATUS = 2;
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);

                        break;

                    case "DECLARACAO EM ANALISE":

                        element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr:nth-child(7) > td:nth-child(2)");
                        data = element.Text;

                        try
                        {
                            import.DT_DATA_DISTR = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            import.DT_DATA_DISTR = null;
                        }

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

                        try
                        {
                            import.DT_DATA_DOSS = DateTime.ParseExact(dataDossie, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            import.DT_DATA_DOSS = null;
                        }

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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
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

                        try
                        {
                            import.DT_DATA_DOSS = DateTime.ParseExact(dataDossie, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            import.DT_DATA_DOSS = null;
                        }

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
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO HISTORICO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await SalvaHistoricoImp(numDeclaracao, historicoImp, import.CD_IMP, import.CD_IMP_STATUS, import.CD_IMP_CANAL);
                        LogController.RegistrarLog(_nome_cliente + " - " + "ATUALIZANDO VISTORIA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await AtualizaVistoria(cd_imp, vistoriaImp);

                        break;

                    default:
                        break;
                }
                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + " ATUALIZANDO STATUS...", eTipoLog.INFO, _cd_bot_exec, "bot");
                await AtualizaStatus(import, cd_imp, historicoImp);
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + numDeclaracao + $", Erro em Acessar. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }

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

                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro de Importacao salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }

                if (resultado.IsSuccessStatusCode)
                {
                    using (var clientH = new HttpClient())
                    {
                        clientH.BaseAddress = new Uri(_urlApiBase);
                        var resultadoHist = await clientH.PutAsJsonAsync($"imp/v1/historico/{0}", historico);
                        resultadoHist.EnsureSuccessStatusCode();

                        LogController.RegistrarLog(_nome_cliente + " - " + "Registro de Historicosalvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + import.TX_NUM_DEC + $" - Erro em AtualizaStatus. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
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
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em AtualizaVistoria. {e.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
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

        private async Task AtualizaTriagem(TriagemBot triagem, string cd_triagem)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = client.PutAsJsonAsync($"imp/v1/triagembot/{cd_triagem}", triagem).Result;
                    resultado.EnsureSuccessStatusCode();

                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro de Triagem salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }                
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - DI: " + triagem.NR_DI + $" - Erro em AtualizaStatus. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }
    }
}
