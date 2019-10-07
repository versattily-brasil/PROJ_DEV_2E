using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
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

        private List<Importacao> registros;

        public Work()
        {
            Console.WriteLine($"********************************************************************************************************************");
            Console.WriteLine("ROBÔ 04 – EXONERAÇÃO DO ICMS");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            Console.WriteLine($"********************************************************************************************************************");
        }

        /// <summary>
        /// Método que inicia o processo de exoneração
        /// </summary>
        /// <returns></returns>
        public async Task ExecutarAsync()
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
                                    Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

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

                                Console.WriteLine("Robô Finalizado !");
                                Console.ReadKey();
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
                Console.WriteLine("Não existe DI's pendente de exoneração.");
            }
        }

        /// <summary>
        /// Método responsável por realizar a tentativa de exonerar as DI's
        /// </summary>
        /// <param name="_driver"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        private async Task ExonerarDIAsync(PhantomJSDriver _driver, Importacao di)
        {
            Console.WriteLine($"=================================================================================================================");
            Console.WriteLine($"Exonerando DI nº {di.TX_NUM_DEC}.");
            
            
            Console.WriteLine($"Autenticação efeturada.");
            Console.WriteLine($"Acessando Endereço: {urlDeclararICMS}");
            _driver.Navigate().GoToUrl(urlDeclararICMS);
            Console.WriteLine($"Seleciona combo: Exoneração do ICMS");
            Select selectTipo = new Select(_driver, By.Id("tp"));
            selectTipo.SelectByText("Exoneração do ICMS");
            Console.WriteLine($"Preenchendo campo DI: {di.TX_NUM_DEC}");
            IWebElement element = _driver.FindElement(By.Id("numDI"));
            element.SendKeys(di.TX_NUM_DEC);
            Console.WriteLine($"Selecionando UF: {di.UF_DI}");
            Select selectUf = new Select(_driver, By.Id("uf"));
            selectUf.SelectByText(di.UF_DI);
            Console.WriteLine($"Registrando...");

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

                Console.WriteLine($"Operação não permitida. {textoAreaErro}");
                            }
            else
            {
                Console.WriteLine("Operação finalizada sem erros.");
                await AtualizarRegistroAsync(di);
            }
        }

        /// <summary>
        /// Recupera todas as di's que ainda não foram exoneradas.
        /// </summary>
        /// <returns></returns>
        private async Task CarregarListaDIAsync()
        {
            Console.WriteLine("Obtendo DI's para exoneração.");

            // monta url para api de importação.
            string urlExoneracao = _urlApiBase + $"imp/v1/importacao/obter-exoneracao-icms";

            // realiza a requisição para a api de importação
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlExoneracao);

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
            Console.WriteLine($"Atualizando DI nº {item.TX_NUM_DEC}.");

            try
            {
                item.DT_DATA_EXO_ICMS = DateTime.Now;

                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);

                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{item.CD_IMP}", item);

                    resultado.EnsureSuccessStatusCode();

                    Console.WriteLine("Registro salvo com sucesso.");
                }
            }
            catch (Exception)
            {

                Console.WriteLine($"Erro ao atualizar a DI nº {item.TX_NUM_DEC}.");
            }
        }
    }
}
