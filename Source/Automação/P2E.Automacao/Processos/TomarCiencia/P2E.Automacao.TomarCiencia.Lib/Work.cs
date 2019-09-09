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
                Log("#####################################################################", nameof(Start));
                Log("############ Inicialização de automação [Tomar Ciência ] ############", nameof(Start));
                Log("#####################################################################", nameof(Start));
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
            LoadCompanyRecords();
            Main();
            Finish();            
        }

        protected void Finish()
        {
            this._driver.Quit();            
            this.service.Dispose();
        }

        protected void LoadCompanyRecords()
        {
            this._driver.Navigate().GoToUrl(_urlPrincipal);

            ReadOnlyCollection<IWebElement> elements = this._driver.FindElements(By.TagName("a"));

            if(elements != null)
            {
                // Looks for the links with the "Inscrições Estaduais"
                foreach (IWebElement tag in elements)
                {
                    // Create the list 
                    if (tag.GetAttribute("href").Contains("?inscricao="))
                    {                        
                        this.Inscricoes.Add(tag.Text.Replace(".", "").Replace("-", ""));
                    }
                }

                if(this.Inscricoes.Count > 0)
                    Log(String.Format("Lista de Inscrições criada com {0} itens.", this.Inscricoes.Count), nameof(LoadCompanyRecords));
            }
            else
            {
                Log("Não foi possível obter a listagem de Inscrições Estaduais", nameof(LoadCompanyRecords));
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
                //create select element object 
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

            foreach (var inscricao in this.Inscricoes)
            {
                this._driver.Navigate().GoToUrl(_urlIncricao + inscricao);
                Log(null, null, true);
                Log(String.Format("Verificando DAIs da Inscrição [{0}]", inscricao), nameof(Main));

                // "Declarações" menu item
                IWebElement declaracoes = this._driver.FindElementById("base_areaGrupo6");
                new Actions(this._driver).MoveToElement(declaracoes).Click().Perform();

                // DAI - Declaração Amazonense de Importação
                IWebElement DAI = this._driver.FindElementById("textoItem102");
                new Actions(this._driver).MoveToElement(DAI).Click().Perform();
                Thread.Sleep(3000);

                // Consulta a Situação de DAIs enviadas
                IWebElement DIOnline = FindDILink();
                if (DIOnline != null)
                    DIOnline.Click();
                Thread.Sleep(2000);

                this.SearchGrayChannelDAIs();

                var debugStop = false;
            }
        }        
    }
}
