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
using System.Timers;
using P2E.Automacao.TomarCiencia.Lib.Model;

namespace P2E.Automacao.TomarCiencia.Lib
{    
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlPrincipal = "https://online.sefaz.am.gov.br/dte/loginSSL.asp"; //"https://online.sefaz.am.gov.br/inicioDte.asp";
        private string _urlIncricao = "https://online.sefaz.am.gov.br/dte/sel_inscricao_pf.asp?inscricao=";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        #endregion

        List<string> Inscricoes = new List<string>();
        List<DAI> DAIList = new List<DAI>();
        List<Empresa> ListaEmpresas = new List<Empresa>();
        PhantomJSDriver _driver = null;        
        PhantomJSDriverService service = null;

        protected void Log(string text, string caller = "", bool newLine = false)
        {
            if (!newLine)
                Console.WriteLine(String.Format("[{0}]", DateTime.Now.ToString()) + " -> " + caller + " - " + text);
            else
                Console.WriteLine(string.Empty);
        }

        public void Start()
        {
            _loginSite = "08281892000158";
            _senhaSite = "2edespachos";

            try
            {
                Log("#####################################################################");
                Log("############ Inicialização de automação [Tomar Ciência ] ############");
                Log("#####################################################################");
                Log(null, null, true);

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

            // Going through the steps
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
        /// Carrega a lista de todas as empresas com suas respectivas Inscrições Estaduais
        /// para agilizar a navegação e manipulação dos dados.
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
                            Log(String.Format("Carregada a empresa {0}, com {1} inscrição(ões) estadual(ais).", empresa.Nome, empresa.IncricoesEstaduais.Count), nameof(CarregaListaEmpresas));
                            leInscricoes = false;
                        }
                    }

                    contagemPar += 2;
                    contagemImpar += 2;
                }
                catch
                {
                    // Não foi encontrado mais nenhuma empresa na estrutura, então sai do loop e encerra o método.
                    leEmpresas = false;
                }
            }

            //ReadOnlyCollection<IWebElement> elements = this._driver.FindElements(By.TagName("a"));

            //if(elements != null)
            //{
            //    // Looks for the links with the "Inscrições Estaduais"
            //    foreach (IWebElement tag in elements)
            //    {


            //        //IWebElement t1 = this._driver.FindElement(By.XPath("//*[@id='areaTrabalho']"));// tr[2]/td[@id='areaTrabalho']/table[2]/tbody/tr/td[3]"));
            //        //IWebElement t2 = this._driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/table/tbody/tr/td/table[5]/tbody/tr[1]/td[3]"));///table[2]/tbody/tr/td[3]"));
            //        //IWebElement razaoSocial = this._driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td[@id='areaTrabalho']/table[2]/tbody/tr/td[3]"));

            //        // Create the list 
            //        if (tag.GetAttribute("href").Contains("?inscricao="))
            //        {                        
            //            this.Inscricoes.Add(tag.Text.Replace(".", "").Replace("-", ""));
            //        }
            //    }

            //    if(this.Inscricoes.Count > 0)
            //        Log(String.Format("Lista de Inscrições criada com {0} itens.", this.Inscricoes.Count), nameof(LoadCompanyRecords));
            //}
            //else
            //{
            //    Log("Não foi possível obter a listagem de Inscrições Estaduais", nameof(LoadCompanyRecords));
            //}
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

        protected void SearchGrayChannelDAIs()
        {
            IWebElement selectChannel = null;
            bool noDAI = false;

            try
            {
                selectChannel = this._driver.FindElementById("canal");
            }
            catch (NoSuchElementException ex)
            {
                Log(ex.Message[0].ToString(), nameof(SearchGrayChannelDAIs));
            }

            Log("Iniciando busca por DAIs...", nameof(SearchGrayChannelDAIs));

            if (selectChannel != null)
            {
                //create select (dropdown) element object 
                var selectElement = new SelectElement(selectChannel);
                //select by value
                //selectElement.SelectByValue("1");
                // select by text (Canal CINZA)
                selectElement.SelectByText("CINZA");
            }

            IWebElement searchButton = null;
            try
            {
                searchButton = this._driver.FindElementById("btPesq");
                searchButton.Click();

                noDAI = CheckForNoDAI();
                if (!noDAI)
                    this.DownloadDAILacre();
            }
            catch (NoSuchElementException ex)
            {                
                Log(ex.Message, nameof(SearchGrayChannelDAIs));                
            }

            bool CheckForNoDAI()
            {
                ReadOnlyCollection<IWebElement> elements;

                try
                {
                    elements = this._driver.FindElements(By.TagName("b"));

                    foreach (IWebElement element in elements)
                    {
                        if (element.Text.Contains("Nenhuma DAI Encontrada"))
                        {
                            Log("Nenhuma DAI Encontrada", nameof(SearchGrayChannelDAIs) + " -> " + nameof(CheckForNoDAI));
                            return true;
                        }
                    }
                }
                catch (NoSuchElementException ex)
                {
                    Log(ex.Message, nameof(SearchGrayChannelDAIs));
                }

                return false;
            }
        }

        protected void GenerateExcelFile()
        {

        }        

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

        protected void Main()
        {
            this._driver.Navigate().GoToUrl(_urlPrincipal);
            
            foreach (Empresa empresa in this.ListaEmpresas)
            {
                foreach (string inscricao in empresa.IncricoesEstaduais)
                {
                    this._driver.Navigate().GoToUrl(_urlIncricao + inscricao);
                    Log(null, null, true);
                    Log(String.Format("Verificando DAIs da Inscrição [{0}]", inscricao), nameof(Main));

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
                }                

                var debugStop = false;
            }
        }        
    }
}
