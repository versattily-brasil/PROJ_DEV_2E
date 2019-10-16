using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.ExonerarIcms.Lib
{
    /// <summary>
    /// Objetivo: Realizar a exoneração de ICMS para um DI.
    /// </summary>
    public class Work
    {
        private string uUrlInicio = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string uUrlbASE = @"https://www1c.siscomex.receita.fazenda.gov.br/";
       
        public string urlDeclararICMS = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/DeclararICMSMenu.do?i=0";
        private string _urlApiBase;
        int _cd_bot_exec;
        int _cd_par;
        private List<Importacao> registros;
        string _nome_cliente;

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "********************************************************************************************************************", eTipoLog.INFO, _cd_bot_exec, "bot");
            LogController.RegistrarLog(_nome_cliente + " - " + "ROBÔ 04 – EXONERAÇÃO DO ICMS", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "********************************************************************************************************************", eTipoLog.INFO, _cd_bot_exec, "bot");
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "********************************************************************************************************************", eTipoLog.INFO, _cd_bot_exec, "bot");
            LogController.RegistrarLog(_nome_cliente + " - " + "ROBÔ 04 – EXONERAÇÃO DO ICMS", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "********************************************************************************************************************", eTipoLog.INFO, _cd_bot_exec, "bot");
        }

        /// <summary>
        /// Método que inicia o processo de exoneração
        /// </summary>
        /// <returns></returns>
        public async Task ExecutarAsync()
        {
            try
            {
                // Carrega todas as DI's que ainda não foram exoneradas.
                await CarregarListaDIAsync();

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
                    {
                        try
                        {
                            ControleCertificados.CarregarCertificado(service);

                            service.AddArgument("test-type");
                            service.AddArgument("no-sandbox");
                            service.HideCommandPromptWindow = true;

                            using (var _driver = new PhantomJSDriver(service))
                            {
                                try
                                {
                                    _driver.Navigate().GoToUrl(uUrlInicio);

                                    // percorre todos os registros para tentar a exoneração.
                                    foreach (var di in registros)
                                    {
                                        LogController.RegistrarLog(_nome_cliente + " - " + "################# DI: " + di.TX_NUM_DEC + " #################", eTipoLog.INFO, _cd_bot_exec, "bot");

                                        List<Thread> threads = new List<Thread>();

                                        var thread = new Thread(() => ExonerarDIAsync(_driver, di));
                                        thread.Start();
                                        threads.Add(thread);

                                        // fica aguardnado todas as threads terminarem...
                                        while (threads.Any(t => t.IsAlive))
                                        {
                                            continue;
                                        }
                                    }

                                    LogController.RegistrarLog(_nome_cliente + " - " + "Robô Finalizado !", eTipoLog.INFO, _cd_bot_exec, "bot");
                                    //Console.ReadKey();
                                }
                                catch (Exception)
                                {
                                    _driver.Close();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's pendente de exoneração.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            Task.WaitAll();
        }

        /// <summary>
        /// Método responsável por realizar a tentativa de exonerar as DI's
        /// </summary>
        /// <param name="_driver"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        private async Task ExonerarDIAsync(PhantomJSDriver _driver, Importacao di)
        {
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "=================================================================================================================", eTipoLog.INFO, _cd_bot_exec, "bot");
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Exonerando DI nº {di.TX_NUM_DEC}.", eTipoLog.INFO, _cd_bot_exec, "bot");


            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Autenticação efeturada.", eTipoLog.INFO, _cd_bot_exec, "bot");
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Acessando Endereço: {urlDeclararICMS}", eTipoLog.INFO, _cd_bot_exec, "bot");
            _driver.Navigate().GoToUrl(urlDeclararICMS);
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Seleciona combo: Exoneração do ICMS", eTipoLog.INFO, _cd_bot_exec, "bot");
            Select selectTipo = new Select(_driver, By.Id("tp"));
            selectTipo.SelectByText("Exoneração do ICMS");
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Preenchendo campo DI: {di.TX_NUM_DEC}", eTipoLog.INFO, _cd_bot_exec, "bot");
            IWebElement element = _driver.FindElement(By.Id("numDI"));
            element.SendKeys(di.TX_NUM_DEC);
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Selecionando UF: {di.UF_DI}", eTipoLog.INFO, _cd_bot_exec, "bot");
            Select selectUf = new Select(_driver, By.Id("uf"));
            selectUf.SelectByText(di.UF_DI);
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Registrando...", eTipoLog.INFO, _cd_bot_exec, "bot");

            element = _driver.FindElement(By.Id("registrar"));
            element.Click();
            string txtInicioBlocoErro = "<!-- MENSAGENS DE ERRO =================================================================================================================================== -->";

            int posicaoInicioErro = _driver.PageSource.ToString().IndexOf(txtInicioBlocoErro);

            if (posicaoInicioErro > 0 && _driver.PageSource.Contains("alert"))
            {
                posicaoInicioErro = posicaoInicioErro + txtInicioBlocoErro.Length;

                var posicaoFimErro = _driver.PageSource.ToString().LastIndexOf(");");

                string textoAreaErro = _driver.PageSource.Substring(posicaoInicioErro, posicaoFimErro - posicaoInicioErro);
                textoAreaErro = textoAreaErro.Replace("\"", "'");
                textoAreaErro = textoAreaErro.Replace("<script charset='utf-8' type='text/javascript'>alert('", "");
                textoAreaErro = textoAreaErro.Replace("'+ '\\n' +''", "");
                textoAreaErro = Regex.Replace(textoAreaErro, @"[^ 0-9a-zA-Z]+", "");
                textoAreaErro = Regex.Replace(textoAreaErro, "(\r\n|\r|\n|\t|\r\n\t)", "");

                LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Operação não permitida. {textoAreaErro}", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
            else
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "Operação finalizada sem erros.", eTipoLog.INFO, _cd_bot_exec, "bot");
                await AtualizarRegistroAsync(di);
            }
        }

        /// <summary>
        /// Recupera todas as di's que ainda não foram exoneradas.
        /// </summary>
        /// <returns></returns>
        private async Task CarregarListaDIAsync()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para exoneração.", eTipoLog.INFO, _cd_bot_exec, "bot");

            // monta url para api de importação.
            string urlExoneracao = _urlApiBase + $"imp/v1/importacao/obter-exoneracao-icms/" + _cd_par;

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = client.GetAsync(urlExoneracao).Result;

                // recupera os registros.
                registros = await result.Content.ReadAsAsync<List<Importacao>>();
            }
        }

        /// <summary>
        /// Atualiza a data de exoneração
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task AtualizarRegistroAsync(Importacao item)
        {
            LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Atualizando DI nº {item.TX_NUM_DEC}.", eTipoLog.INFO, _cd_bot_exec, "bot");

            try
            {
                item.DT_DATA_EXO_ICMS = DateTime.Now;

                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);

                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{item.CD_IMP}", item);

                    resultado.EnsureSuccessStatusCode();

                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
            catch (Exception)
            {

                LogController.RegistrarLog(_nome_cliente + " - " + _nome_cliente + " - " + "Erro ao atualizar a DI nº {item.TX_NUM_DEC}.", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
        }
    }
}
