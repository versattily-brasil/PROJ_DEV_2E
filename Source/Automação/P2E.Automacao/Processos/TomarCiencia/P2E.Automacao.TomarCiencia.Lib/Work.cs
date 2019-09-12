using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using P2E.Automacao.Shared.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using P2E.Automacao.TomarCiencia.Lib.Model;
using static P2E.Automacao.TomarCiencia.Lib.Entities.Importacao;
using System.Linq;

namespace P2E.Automacao.TomarCiencia.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlPrincipal = @"https://online.sefaz.am.gov.br/dte/loginSSL.asp"; //"https://online.sefaz.am.gov.br/inicioDte.asp";
        private string _urlIncricao = @"https://online.sefaz.am.gov.br/dte/sel_inscricao_pf.asp?inscricao=";
        private string _urlConsultaDI = @"https://online.sefaz.am.gov.br/sinf2004/DI/pagDIOnline.asp?numPagina="; //"https://online.sefaz.am.gov.br/sinf2004/DI/consultaDIOnline.asp";                
        private string _urlApiBase;
        private List<TBImportacao> ListaProcessosBD;
        #endregion

        List<string> Inscricoes = new List<string>();
        List<DAI> DAIList = new List<DAI>();
        List<Empresa> ListaEmpresas = new List<Empresa>();
        PhantomJSDriver _driver = null;        
        PhantomJSDriverService service = null;

        public Work()
        {            
            Log("############ Inicialização de automação [Tomar Ciência] ############");            
            Log(null, null, true);

            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        protected void Log(string text, string caller = "", bool newLine = false)
        {
            if (!newLine)
                Console.WriteLine(String.Format("[{0}]", DateTime.Now.ToString()) + " -> " + caller + " - " + text);
            else
                Console.WriteLine(string.Empty);
        }

        public void Start()
        {
            try
            {                
                this.service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory());                

                // Carrega o certificado
                ControleCertificados.CarregarCertificado(this.service);

                this.service.AddArgument("test-type");
                this.service.AddArgument("no-sandbox");
                this.service.HideCommandPromptWindow = true;

                this._driver = new PhantomJSDriver(this.service);
            }
            catch
            {
                var error = true;
            }

            // Executa o passo-a-passo
            //await CarregarListaDIAsync();
            CarregaListaEmpresas();
            Main();
            Finish();            
        }

        protected void Finish()
        {
            this._driver.Quit();            
            this.service.Dispose();
        }

        /// <summary>
        /// Carrega, do site, a lista de todas as empresas com suas respectivas Inscrições Estaduais
        /// para agilizar a navegação e manipulação dos dados.
        /// O número da Inscrição Estadual será posteriormente usado para acesso à URL "_urlIncricao".
        /// </summary>
        protected void CarregaListaEmpresas()
        {
            this._driver.Navigate().GoToUrl(_urlPrincipal);

            bool leEmpresas = true;
            bool leInscricoes = true;
            int contagemImpar = 1;
            int contagemPar = 2;
            int contagemInscricao = 1;

            string razaoSocial;
            string CNPJ;
            string inscricaoEstadual;
            while (leEmpresas)
            {
                Empresa empresa = new Empresa();
                try
                {
                    // Carrega os dados básicos da empresa à partir de busca via XPath.
                    razaoSocial = this._driver.FindElement(By.XPath(String.Format("/html/body/table/tbody/tr[2]/td/table/tbody/tr/td/table[{0}]/tbody/tr[1]/td[3]", contagemImpar))).Text;
                    CNPJ = this._driver.FindElement(By.XPath(String.Format("/html/body/table/tbody/tr[2]/td/table/tbody/tr/td/table[{0}]/tbody/tr[2]/td[3]", contagemImpar))).Text;

                    empresa.Nome = razaoSocial;
                    empresa.CNPJ = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
                    empresa.IncricoesEstaduais = new List<string>();

                    ListaEmpresas.Add(empresa);

                    leInscricoes = true;
                    contagemInscricao = 1;

                    while (leInscricoes)
                    {
                        try
                        {
                            // Carrega todas as inscrições estaduais da empresa à partir de busca via XPath.
                            inscricaoEstadual = this._driver.FindElement(By.XPath(String.Format("/html/body/table/tbody/tr[2]/td/table/tbody/tr/td/table[{0}]/tbody/tr[{1}]/td[2]", contagemPar, contagemInscricao))).Text;
                            empresa.IncricoesEstaduais.Add(inscricaoEstadual.Replace(".", "").Replace("-", ""));

                            contagemInscricao += 1;
                        }
                        catch
                        {
                            // Não foi encontrado mais nenhuma inscrição estdual na estrutura, então sai do loop 
                            // e segue para a próxima empresa.
                            Log(String.Format("Empresa {0}: {1} inscrição(ões) estadual(ais).", empresa.Nome, empresa.IncricoesEstaduais.Count), nameof(CarregaListaEmpresas));                            
                            leInscricoes = false;
                        }
                    }

                    contagemPar += 2;
                    contagemImpar += 2;
                }
                catch
                {
                    // Não foi encontrado mais nenhuma empresa na estrutura, então sai do loop e encerra o método.
                    Log(String.Format("{0} empresas carregadas.", this.ListaEmpresas.Count));
                    leEmpresas = false;
                }
            }
        }

        /// <summary>
        /// Carrega a lista de DIs presentes no Banco de Dados para verificação
        /// se estão processadas ou não.
        /// </summary>
        /// <returns></returns>
        private async Task CarregarListaDIAsync()
        {
            string urlTomarCiencia = _urlApiBase + $"imp/v1/importacao/todos";

            using (var client = new HttpClient())
            {
                Log("Carregando lista de DAI's já registradas...");
                var result = await client.GetAsync(urlTomarCiencia);
                ListaProcessosBD = await result.Content.ReadAsAsync<List<TBImportacao>>();                
            }
        }

        // Verifica se um Alert box foi exibido
        public bool isAlertPresent()
        {
            try
            {
                this._driver.SwitchTo().Alert();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected IWebElement FindDILink()
        {
            ReadOnlyCollection<IWebElement> elements = null;

            try
            {
                elements = this._driver.FindElements(By.TagName("a"));
                // Search for the link
                foreach (IWebElement tag in elements)
                {
                    if (tag.GetAttribute("href").Contains("consultaDIOnline.asp"))
                    {
                        Log("Link para a DI encontrado.", nameof(FindDILink));
                        return tag;
                    }
                }
            }
            catch (NoSuchElementException ex)
            {
                string errorMessage = JsonConvert.DeserializeObject(ex.Message).ToString();
                Log(errorMessage, nameof(FindDILink));
            }            

            return null;
        }

        // Could not use yet.

        //public static void WaitForLoad(IWebDriver driver, int timeoutSec = 15)
        //{
        //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        //    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
        //    wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        //}        

        protected void GenerateExcelFile()
        {
            // Gerar o arquivo Excel com as DAIS Tomadas Ciência
            // para os clientes Samsung e Ventisol
        }        

        /// <summary>
        /// Salva o arquivo de Lacre como PDF.
        /// </summary>
        protected void DownloadDAILacre()
        {
            ReadOnlyCollection<IWebElement> elements;
            try
            {
                elements = this._driver.FindElements(By.ClassName("link"));

                foreach (IWebElement element in elements)
                {
                    if(element.Text.Contains("[Imp. de Lacre]"))
                    {
                        new Actions(this._driver).MoveToElement(element).Click().Perform();
                        var contentLacre = this._driver.PageSource;
                        // Salvar como PDF
                        // PDFSharp talvez?!?!
                    }
                }
            }
            catch (NoSuchElementException ex)
            {
                Log(ex.Message, nameof(DownloadDAILacre));
            }
        }

        protected void RegistrarProcessoTomadoCiencia(DAI processo)
        {
            // Registrar o processo na base de dados
        }

        /// <summary>
        /// Principal método do Robô. Aqui a sessão para cada Inscrição Estadual é estabelecida
        /// ao acessar a "_urlInscriçao"
        /// </summary>
        protected void Main()
        {
            // Talvez esse passo de acesso a esta URL nem seja mais necessário.
            this._driver.Navigate().GoToUrl(_urlPrincipal);

            int paginaInicial = 1;
            int numeroPaginas;
            List<DAI> listaDAIProcessadas = new List<DAI>();

            foreach (Empresa empresa in this.ListaEmpresas)
            {
                foreach (string inscricao in empresa.IncricoesEstaduais)
                {
                    // Garante que a sessão corrente esteja no contexto da Inscrição Estadual atual
                    this._driver.Navigate().GoToUrl(this._urlIncricao + inscricao);
                    Log(null, null, true);
                    Log(String.Format("Verificando DAIs da Inscrição [{0}]", inscricao), nameof(Main));

                    // Segue diretamente à listagem das DAI's da Inscrição Estadual
                    // selecionada no passo anterior
                    this._driver.Navigate().GoToUrl(this._urlConsultaDI + paginaInicial);
                    Thread.Sleep(5000);

                    // Captura o total de páginas da listagem de DAI's da Inscrição Estadual atual
                    string totalPaginas = this._driver.FindElement(By.XPath("/html/body/table[1]/tbody/tr/td/table/tbody/tr/td[3]")).Text;
                    numeroPaginas = Convert.ToInt32(totalPaginas.Substring(totalPaginas.LastIndexOf('/') + 1).Trim());

                    // Coluna onde o link "[Tomar Ciência] é exibido.
                    //ReadOnlyCollection<IWebElement> listaDAIs = this._driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[5]/a"));

                    // Página a página, busca pelas DAI's que estejam pendentes de "Tomar Ciência".
                    for (int pag = 1; pag < numeroPaginas; pag++)
                    {
                        ReadOnlyCollection<IWebElement> listaDAIs = this._driver.FindElements(By.CssSelector("a[onclick^='tomarCiencia']"));
                        for (int i = 1; i < listaDAIs.Count; i++) 
                        {
                            string numeroDAI = this._driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr[i]/td[1]")).Text;
                            DAI processo = new DAI();

                            // Dispara o evento "onclick" do link que, por sua vez, exibe um "alert" para 
                            // confirmação da operação.
                            new Actions(this._driver).MoveToElement(listaDAIs[i]).Click().Perform();
                            // Verifica se o Alert está sendo exibido (tenho minhas dúvidas sobre a eficácia disso)
                            if (isAlertPresent())
                            {
                                // Captura o texto do Alert apenas para propósitos de debug.
                                string alertText = this._driver.SwitchTo().Alert().Text;
                                // Confirma a operação clicando no botão OK do Alert.
                                this._driver.SwitchTo().Alert().Accept();
                            }                            
                        }
                    }

                    /*
                    // "Declarações" menu item
                    IWebElement declaracoes = this._driver.FindElementById("base_areaGrupo6");
                    new Actions(this._driver).MoveToElement(declaracoes).Click().Perform();

                    // DAI - Declaração Amazonense de Importação
                    // Busca o link para a DAI na página de Declarações, dentro da Inscrição Estadual corrente.
                    IWebElement DAI = this._driver.FindElementById("textoItem102");
                    new Actions(this._driver).MoveToElement(DAI).Click().Perform();
                    Thread.Sleep(3000);

                    // Consulta a Situação de DAIs enviadas
                    IWebElement DIOnline = FindDILink();
                    if (DIOnline != null)
                        DIOnline.Click();
                    Thread.Sleep(2000);

                    this.SearchGrayChannelDAIs();
                    */
                }

                var debugStop = false;
            }
        }        
    }
}
