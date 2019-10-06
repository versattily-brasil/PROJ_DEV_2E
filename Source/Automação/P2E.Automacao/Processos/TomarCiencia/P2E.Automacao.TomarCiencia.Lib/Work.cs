using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.TomarCiencia.Lib.Model;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.TomarCiencia.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlPrincipal = @"https://online.sefaz.am.gov.br/inicioDte.asp"; //"https://online.sefaz.am.gov.br/dte/loginSSL.asp"; 
        private string _urlIncricao = @"https://online.sefaz.am.gov.br/dte/sel_inscricao_pf.asp?inscricao=";
        private string _urlConsultaDI = @"https://online.sefaz.am.gov.br/sinf2004/DI/pagDIOnline.asp?numPagina="; //"https://online.sefaz.am.gov.br/sinf2004/DI/consultaDIOnline.asp";                
        private string _urlApiBase;
        private string _urlDocLacre = @"https://online.sefaz.am.gov.br/sinf2004/DI/rel_lacre.asp?idDi=";
        private List<Importacao> ListaProcessosBD;
        #endregion

        List<string> Inscricoes = new List<string>();
        List<DAI> DAIList = new List<DAI>();
        List<Empresa> ListaEmpresas = new List<Empresa>();
        PhantomJSDriver _driver = null;
        PhantomJSDriver _driverTemp;
        PhantomJSDriverService service = null;
        OpenQA.Selenium.IWebElement element = null;

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
                this.service = PhantomJSDriverService.CreateDefaultService();

                // Carrega o certificado
                ControleCertificados.CarregarCertificado(service);

                this.service.AddArgument("test-type");
                this.service.AddArgument("no-sandbox");
                this.service.HideCommandPromptWindow = true;

                this._driver = new PhantomJSDriver(service);
                this._driverTemp = new PhantomJSDriver(service);
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
            element = _driver.FindElementByCssSelector("#areaTrabalho > table > tbody > tr > td > table > tbody > tr:nth-child(2) > td > table > tbody > tr > td:nth-child(3) > a > img");
            element.Click();

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
                ListaProcessosBD = await result.Content.ReadAsAsync<List<Importacao>>();
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

        protected void GenerateExcelFile()
        {
            // Gerar o arquivo Excel com as DAIS Tomadas Ciência
            // para os clientes Samsung e Ventisol
        }

        /// <summary>
        /// Salva o arquivo de Lacre como PDF.
        /// </summary>
        protected bool DownloadDAILacre()
        {
            var retorno = false;

            ReadOnlyCollection<IWebElement> elements = null;
            try
            {
                elements = this._driver.FindElementsByClassName("link"); 

                foreach (IWebElement element in elements)
                {
                    if (element.Text.Contains("[Imp. de Lacre]"))
                    {
                        var aux = _driver.PageSource;

                        var position = aux.IndexOf("[Imp. de Lacre]");

                        var nroLacre = aux.Substring((position-13), 7);                                                

                        _driverTemp.Navigate().GoToUrl(_urlDocLacre + nroLacre);
                        Thread.Sleep(3000);

                        var print = capturaImagem(_driverTemp, nroLacre);

                        retorno = true;
                    }
                }

                return retorno;
            }
            catch (NoSuchElementException ex)
            {
                Log(ex.Message, nameof(DownloadDAILacre));
                return false;
            }
        }

        public bool Screenshot(IWebDriver driver, string screenshotsPasta)
        {
            try
            {
                ITakesScreenshot camera = driver as ITakesScreenshot;
                Screenshot foto = camera.GetScreenshot();
                foto.SaveAsFile(screenshotsPasta+".png", ScreenshotImageFormat.Png);

                System.Drawing.Image png = System.Drawing.Image.FromFile(screenshotsPasta + ".png");
                using (var bitmap = new Bitmap(png.Width, png.Height))
                {
                    bitmap.SetResolution(, png.HorizontalResolution, png.VerticalResolution);

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.White);
                        g.DrawImageUnscaled(png, 0, 0);
                    }

                    bitmap.Save(screenshotsPasta + ".jpg", ImageFormat.Jpeg);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool capturaImagem(PhantomJSDriver _driver, string numero)
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", numero + "-CapturaTela");

                var retornoPrint = Screenshot(_driver, arquivoPath);

                if (retornoPrint)
                {
                    ConvertImagePDF(numero + "-CapturaTela.jpg",numero);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ConvertImagePDF( string nomePrint, string lacre)
        {
            try
            {
                iTextSharp.text.Document Doc = new iTextSharp.text.Document(PageSize.LETTER, 20, 20, 20, 20);

                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\");

                string PDFOutput = Path.Combine(arquivoPath, "Lacre-"+lacre+".pdf");
                PdfWriter writer = PdfWriter.GetInstance(Doc, new FileStream(PDFOutput, FileMode.Create, FileAccess.Write, FileShare.Read));

                Doc.Open();


                foreach (string F in System.IO.Directory.GetFiles(arquivoPath, nomePrint))
                {
                    Doc.NewPage();                   
                    Doc.Add(new iTextSharp.text.Jpeg(new Uri(new FileInfo(F).FullName)));
                }

                Doc.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
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
            //this._driver.Navigate().GoToUrl(_urlPrincipal);

            int paginaInicial = 1;
            int numeroPaginas;
            List<DAI> listaDAIProcessadas = new List<DAI>();
            int r = 0; // Índice das células do arquivo excel ( xls )
            int j = -1; // Índice das colunas do arquivo excel ( xls )
            ExcelPackage xlsDoc = null;
            int colDI = 0;
            int colData = 0;
            int colSinal = 0;
            ExcelWorksheet sheet = null;

            foreach (Empresa empresa in this.ListaEmpresas)
            {
                Log("Empresa - " + empresa.Nome.Trim());
                //CRIA PLANILHA PARA SAMSUNG E VENTISOL
                if (empresa.Nome.Contains("SAMSUNG") || empresa.Nome.Contains("VENTISOL"))
                {
                    Log("*****Criando Planilha*****");
                    xlsDoc = new ExcelPackage();

                    xlsDoc.Workbook.Properties.Title = "2E";

                    r = 0;
                    r++;
                    colDI = 1;
                    colData = 2;
                    colSinal = 3;
                }

                foreach (string inscricao in empresa.IncricoesEstaduais)
                {
                    //CRIA PLANILHA PARA SAMSUNG E VENTISOL
                    if (empresa.Nome.Contains("SAMSUNG") || empresa.Nome.Contains("VENTISOL"))
                    {
                        sheet = xlsDoc.Workbook.Worksheets.Add(inscricao.Trim());
                        sheet.Name = inscricao.Trim();

                        r++;
                        sheet.Cells[r, colDI].Value = "Inscrição Nº " + inscricao.Trim();
                        r++;

                        sheet.Cells[r, colDI].Value = "Numero DI";
                        sheet.Cells[r, colData].Value = "Data";
                        sheet.Cells[r, colSinal].Value = "Canal";

                        r++;
                    }

                    // Garante que a sessão corrente esteja no contexto da Inscrição Estadual atual
                    this._driver.Navigate().GoToUrl(this._urlIncricao + inscricao);
                    Log(null, null, true);
                    Log(String.Format("Verificando DAIs da Inscrição [{0}]", inscricao), nameof(Main));

                    // Segue diretamente à listagem das DAI's da Inscrição Estadual
                    // selecionada no passo anterior
                    this._driver.Navigate().GoToUrl(this._urlConsultaDI + paginaInicial);
                    Thread.Sleep(5000);

                    
                    string totalPaginas = "0";

                    try
                    {
                        // Captura o total de páginas da listagem de DAI's da Inscrição Estadual atual
                        totalPaginas = this._driver.FindElement(By.XPath("/html/body/table[1]/tbody/tr/td/table/tbody/tr/td[3]")).Text;
                        numeroPaginas = Convert.ToInt32(totalPaginas.Substring(totalPaginas.LastIndexOf('/') + 1).Trim());
                    }
                    catch (Exception)
                    {
                        numeroPaginas = 0;
                        string mensagem = this._driver.FindElement(By.XPath("/html/body/table/tbody/tr/td[2]/b")).Text;
                        Log("######" + mensagem + "######");
                    }

                    // Coluna onde o link "[Tomar Ciência] é exibido.
                    //ReadOnlyCollection<IWebElement> listaDAIs = this._driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[5]/a"));

                    // Página a página, busca pelas DAI's que estejam pendentes de "Tomar Ciência".
                    for (int pag = 1; pag < numeroPaginas; pag++)
                    {
                        Log("Navegando para a página " + pag);
                        this._driver.Navigate().GoToUrl(this._urlConsultaDI + pag);
                        Log("Loading de 5 seg.");
                        Thread.Sleep(5000);

                        var retornoPrint = DownloadDAILacre();

                        if (retornoPrint)
                        {
                            //this._driver.Navigate().GoToUrl(this._urlConsultaDI + pag);
                            //Thread.Sleep(5000);
                        }

                        r++;
                        sheet.Cells[r, colDI].Value = "Pagina " + pag;
                        r++;
                        r++;

                        if (empresa.Nome.Contains("SAMSUNG") || empresa.Nome.Contains("VENTISOL"))
                        {
                            ReadOnlyCollection<IWebElement> elements;
                            try
                            {
                                elements = _driver.FindElements(By.ClassName("dg_ln_impar"));

                                foreach (IWebElement element in elements)
                                {
                                    var nroDI = element.Text.Substring(0, 9);
                                    Log("DI= " + nroDI);

                                    var data = element.Text.Substring(10, 10);
                                    var status = "";

                                    sheet.Cells[r, colDI].Value = nroDI;
                                    sheet.Cells[r, colData].Value = data;

                                    if (element.Text.Contains("Parametriza"))
                                    {
                                        status = "Pendente de parametrização";
                                        sheet.Cells[r, colSinal].Value = status;
                                    }
                                    else
                                    {
                                        status = "Verde";
                                        sheet.Cells[r, colSinal].Value = status;

                                        using (var range = sheet.Cells[r, colSinal])
                                        {
                                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                                        }
                                    }

                                    r++;
                                }

                                elements = _driver.FindElements(By.ClassName("dg_ln_par"));

                                foreach (IWebElement elemento in elements)
                                {
                                    var nroDI = elemento.Text.Substring(0, 9);
                                    Log("DI= " + nroDI);

                                    var data = elemento.Text.Substring(10, 10);
                                    var status = "";

                                    sheet.Cells[r, colDI].Value = nroDI;
                                    sheet.Cells[r, colData].Value = data;

                                    if (elemento.Text.Contains("Parametriza"))
                                    {
                                        status = "Pendente de parametrização";
                                        sheet.Cells[r, colSinal].Value = status;
                                    }
                                    else
                                    {
                                        status = "Verde";
                                        sheet.Cells[r, colSinal].Value = status;

                                        using (var range = sheet.Cells[r, colSinal])
                                        {
                                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                                        }
                                    }

                                    r++;
                                }
                            }
                            catch (NoSuchElementException ex)
                            {

                            }
                        }

                        try
                        {
                            ReadOnlyCollection<IWebElement> listaDAIs = this._driver.FindElements(By.CssSelector("a[onclick^='tomarCiencia']"));
                            for (int p = 1; p < listaDAIs.Count; p++)
                            {
                                string numeroDAI = this._driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr[i]/td[1]")).Text;
                                DAI processo = new DAI();

                                // Dispara o evento "onclick" do link que, por sua vez, exibe um "alert" para 
                                // confirmação da operação.
                                new Actions(this._driver).MoveToElement(listaDAIs[p]).Click().Perform();
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
                        catch (Exception)
                        {

                        }
                    }

                    r = 1;
                    colDI = 1;
                    colData = 2;
                    colSinal = 3;



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

                if (empresa.Nome.Contains("SAMSUNG") || empresa.Nome.Contains("VENTISOL"))
                {
                    //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                    if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                    {
                        System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                    }

                    var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                    string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-TomarCiencia.xlsx");

                    File.WriteAllBytes(arquivoPath, xlsDoc.GetAsByteArray());
                }

                var debugStop = false;
            }
        }
    }
}
