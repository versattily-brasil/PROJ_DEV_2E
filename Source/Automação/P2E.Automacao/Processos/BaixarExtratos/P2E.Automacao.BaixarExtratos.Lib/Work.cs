using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using SimpleBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;


namespace P2E.Automacao.BaixarExtratos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        public string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";//?nrDeclaracao=19/0983204-0&consulta=true";
        public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?";
        // public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?nrDeclaracao=19%2F0983204-0&consulta=true";
        private string _urlApiBase;
        private List<Importacao> registros;

        #endregion
        public Work()
        {
            Console.WriteLine("#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ");
            //_urlApiBase = "http://localhost:7000/";
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

        }

        public async Task ExecutarAsync()
        {
            try
            {
                Console.WriteLine("Obtendo DI's para Baixar Extrato.");
                await CarregarListaDIAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CarregarListaDIAsync()
        {
            try
            {
                string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-pdf-xml";

                using (var client = new HttpClient())
                {
                    Console.WriteLine("ABRINDO CONEXAO...");
                    var result = await client.GetAsync(urlAcompanha);
                    var aux = await result.Content.ReadAsStringAsync();
                    registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

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
                                    _driver.Navigate().GoToUrl(_urlSite);
                                    Console.WriteLine(_driver.Url);

                                    foreach (var di in registros)
                                    {
                                        Console.WriteLine("################## DI: " + di.TX_NUM_DEC + " ##################");

                                        List<Thread> threads = new List<Thread>();

                                        var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver, di, di.CD_IMP.ToString()));
                                        thread.Start();
                                        threads.Add(thread);

                                        // fica aguardnado todas as threads terminarem...
                                        while (threads.Any(t => t.IsAlive))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    _driver.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string cd_imp)
        {
            try
            {
                Console.WriteLine("Inciando processo de navegação...");

                //página da consulta DI.
                _driver.Navigate().GoToUrl(_urlConsultaDI);
                Console.WriteLine(_driver.Url);

                //obtendo o campo de numero de declaração.
                IWebElement element = _driver.FindElementById("nrDeclaracao");

                Console.WriteLine("inserindo o numero da declaração");
                element.SendKeys(numero);

                Console.WriteLine("Acionando o Click no enviar.");
                Thread.Sleep(1000);

                element = _driver.FindElement(By.Name("enviar"));
                Thread.Sleep(1000);
                element.Click();

                Thread.Sleep(1000);

                //indo para a página de consulta de declaração de importação.
                _driver.FindElement(By.Id("btnRegistrarDI")).Click();

                Thread.Sleep(1000);

                Console.WriteLine("Baixando o Extrato - PDF.");
                Thread.Sleep(1000);

                var numeroDec = numero.Substring(0, 2) + "%2F" +
                                numero.Substring(2, 7) + "-" +
                                numero.Substring(9, 1);

                var returnoPDF = DownloadExtratoPDF(_driver, _urlDownloadPDF + "?nrDeclaracao=" + numeroDec);

                import.OP_EXTRATO_PDF = returnoPDF ? 1 : 0;

                Console.WriteLine("Baixando o Extrato - XML.");

                var returnoXML = DownloadExtratoXML(numeroDec);

                import.OP_EXTRATO_XML = returnoXML ? 1 : 0;

                await AtualizaExtratoPdfXml(import, cd_imp);
            }
            catch (Exception e)
            {
                _driver.Close();
            }
        }

        private async Task AtualizaExtratoPdfXml(Importacao import, string cd_imp)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);
                    resultado.EnsureSuccessStatusCode();

                    Console.WriteLine("Registro salvo com sucesso.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.");
            }
        }

        /// <summary>
        /// Realiza o download o arquivo de Extrato.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        protected bool DownloadExtratoPDF(PhantomJSDriver driver, string _url)
        {
            try
            {
                var certificado = ControleCertificados.GetClientCertificate(); ;

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
                    Thread.Sleep(1000);
                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool DownloadExtratoXML(string numero)
        {
            try
            {
                //var certificado = ControleCertificados.FindClientCertificate("511d1904137f8ed4");
                var certificado = ControleCertificados.GetClientCertificate();
                using (var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(certificado))
                {
                    var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                    //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                    if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                    {
                        System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                    }

                    string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.xml");


                    if (!File.Exists(arquivoPath))
                    {
                        driver._my.Navigate(_urlSite);
                        driver._my.Navigate(_urlConsultaDI);
                        driver._my.Navigate(new Uri(_urlDownloadXML),
                            "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + numero + "&numeroRetificacao=&enviar=Consultar",
                            "application/x-www-form-urlencoded");

                        driver.FindElement(By.Id("nrDeclaracaoXml")).SendKeys(numero);
                        driver.FindElement(By.Name("ConsultarDiXmlForm")).Submit();

                        Thread.Sleep(5000);

                        File.WriteAllBytes(arquivoPath, ConvertToByteArray(driver.PageSource));
                    }

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //public static bool DownloadExtratoXML(string NrDi)
        //{
        //    try
        //    {
        //        //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
        //        if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
        //        {
        //            System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
        //        }

        //        var sNomeArquivo = Path.Combine("C:\\Versatilly\\", NrDi + "-Extrato.xml");

        //        sNomeArquivo = sNomeArquivo.Replace("/", "_");
        //        string sSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        //        Uri sUri = new Uri(sSite);
        //        NrDi = NrDi.Replace("/", "");
        //        NrDi = NrDi.Replace("-", "");
        //        var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");
        //        Browser browser = new Browser(certificado);
        //        browser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
        //        //browser.KeepAlive = true;
        //        browser.Navigate(new Uri("https://www1.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/login_cert.jsp"));
        //        browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do"));
        //        // PEGA NOME DO CLIENTE
        //        string sDiAlterada = string.Empty;
        //        if (!File.Exists(sNomeArquivo))
        //        {
        //            browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do"));
        //            // pdf extrato
        //            browser.Navigate(new Uri("https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDI.do"),
        //                "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + NrDi + "&numeroRetificacao=&enviar=Consultar",
        //                "application/x-www-form-urlencoded");
        //            browser.Find("input", FindBy.Id, "nrDeclaracaoXml").Value = NrDi;
        //            browser.Find("form", FindBy.Name, "ConsultarDiXmlForm").SubmitForm();
        //            for (Int32 jj = 0; jj <= 8; jj++)
        //            {
        //                Thread.Sleep(200);
        //            }
        //            File.WriteAllBytes(sNomeArquivo, ConvertToByteArray(browser.CurrentHtml));
        //        }
        //        browser.Close();
        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Console.WriteLine(exc.Message);
        //        throw;
        //    }
        //}

        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
            return arr;
        }
    }
}


#region chromedriver

//using Newtonsoft.Json;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.PhantomJS;
//using P2E.Automacao.BaixarExtratos.Lib.Entities;
//using P2E.Automacao.Shared.Extensions;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Cryptography.X509Certificates;
//using System.Threading;
//using System.Threading.Tasks;

//namespace P2E.Automacao.BaixarExtratos.Lib
//{
//    public class Work
//    {
//        #region Variaveis Estáticas

//        public string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
//        public string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
//        public string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";
//        public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do";
//        //public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?nrDeclaracao=19%2F0983204-0&consulta=true";
//        private string _urlApiBase;
//        private List<Importacao> registros;

//        #endregion
//        public Work()
//        {
//            Console.WriteLine("#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ");
//            //_urlApiBase = "http://localhost:7000/";
//            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

//        }

//        public async Task ExecutarAsync()
//        {
//            Console.WriteLine("Obtendo DI's para Baixar Extrato.");
//            await CarregarListaDIAsync();
//        }

//        private async Task CarregarListaDIAsync()
//        {
//            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-pdf-xml";

//            using (var client = new HttpClient())
//            {
//                Console.WriteLine("ABRINDO CONEXAO...");
//                var result = await client.GetAsync(urlAcompanha);
//                var aux = await result.Content.ReadAsStringAsync();
//                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

//                if (registros != null && registros.Any())
//                {
//                    try
//                    {
//                        using (var service = /*PhantomJSDriverService*/ ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
//                        {
//                            ChromeOptions options = new ChromeOptions();

//                            Console.WriteLine("CARREGANDO O CERTIFICADO...");
//                            ControleCertificados.CarregarCertificado(options);

//                            options.AddArgument("test-type");
//                            options.AddArgument("no-sandbox");
//                            service.HideCommandPromptWindow = true;

//                            using (var _driver = new /*PhantomJSDriver*/ ChromeDriver(service, options))
//                            {
//                                _driver.Navigate().GoToUrl(_urlSite);
//                                Console.WriteLine(_driver.Url);

//                                foreach (var di in registros)
//                                {
//                                    Console.WriteLine("################## DI: " + di.TX_NUM_DEC + " ##################");

//                                    List<Thread> threads = new List<Thread>();

//                                    var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver, di, di.CD_IMP.ToString()));
//                                    thread.Start();
//                                    threads.Add(thread);

//                                    // fica aguardnado todas as threads terminarem...
//                                    while (threads.Any(t => t.IsAlive))
//                                    {
//                                        continue;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine("erro no driver " + e.Message.Trim());
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
//                }
//            }
//        }

//        private async Task Acessar(string numero, /*PhantomJSDriver*/ ChromeDriver _driver, Importacao import, string cd_imp)
//        {
//            try
//            {
//                Console.WriteLine("Inciando processo de navegação...");

//                ////navega para primeira url.
//                ////onde é realizado o login através do certificado.
//                //_driver.Navigate().GoToUrl(_urlSite);
//                //Console.WriteLine(_driver.Url);

//                //Navega para seguinda url.
//                //página da consulta DI.
//                _driver.Navigate().GoToUrl(_urlConsultaDI);
//                Console.WriteLine(_driver.Url);
//                Thread.Sleep(2000);
//                //obtendo o campo de numero de declaração.
//                IWebElement element = _driver.FindElementById("nrDeclaracao");

//                Console.WriteLine("inserindo o numero da declaração");
//                element.SendKeys(numero);

//                Console.WriteLine("Acionando o Click no enviar.");
//                // Thread.Sleep(1000);

//                element = _driver.FindElement(By.Name("enviar"));
//                //  Thread.Sleep(1000);
//                element.Click();

//                // Thread.Sleep(1000);

//                //indo para a página de consulta de declaração de importação.
//                _driver.FindElement(By.Id("btnRegistrarDI")).Click();

//                import.OP_EXTRATO_PDF = 1;

//                _driver.FindElement(By.Id("consultarXmlDi")).Click();

//                import.OP_EXTRATO_XML = 0;

//                await AtualizaExtratoPdfXml(import, cd_imp);









//                //Thread.Sleep(1000);

//                //string numeroDec = numero.Substring(0, 2) + "%2F" +
//                //                numero.Substring(2, 7) + "-" +
//                //                numero.Substring(9, 1);

//                //Console.WriteLine("Baixando o Extrato - PDF.");
//                //Thread.Sleep(1000);

//                //var returnoPDF = DownloadExtratoPDF(_driver, _urlDownloadPDF + "?nrDeclaracao=" + numeroDec);

//                ////ESCOLHE A NOVA JANELA ABERTA COM O CLIQUE
//                //try
//                //{
//                //    element = _driver.FindElement(By.Id("consultarXmlDi"));
//                //    element.Click();

//                //    _driver.SwitchTo().Window(_driver.WindowHandles[0]);

//                //    element.Submit();

//                //    var returnoXML = DownloadExtratoXML(_driver, _urlDownloadXML );
//                //}
//                //catch (Exception e)
//                //{
//                //    Console.WriteLine(e.ToString());
//                //}

//            }
//            catch (Exception e)
//            {
//                _driver.Close();
//            }
//        }

//        private async Task AtualizaExtratoPdfXml(Importacao import, string cd_imp)
//        {
//            try
//            {
//                HttpResponseMessage resultado;

//                using (var client = new HttpClient())
//                {
//                    client.BaseAddress = new Uri(_urlApiBase);
//                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);
//                    resultado.EnsureSuccessStatusCode();

//                    Console.WriteLine("Registro salvo com sucesso.");
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine($"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.");
//            }
//        }

//        /// <summary>
//        /// Realiza o download o arquivo de Extrato.
//        /// </summary>
//        /// <param name="driver"></param>
//        /// <returns></returns>
//        protected bool DownloadExtratoPDF(/*PhantomJSDriver*/ ChromeDriver driver, string _url)
//        {
//            try
//            {
//                var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");

//                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

//                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
//                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
//                {
//                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
//                }

//                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.pdf");

//                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
//                {
//                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
//                    Thread.Sleep(1000);
//                    myWebClient.DownloadFile(_url, arquivoPath);
//                }

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        protected bool DownloadExtratoXML(/*PhantomJSDriver*/ ChromeDriver driver, string _url)
//        {
//            try
//            {
//                var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");

//                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

//                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
//                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
//                {
//                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
//                }

//                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.xml");

//                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
//                {
//                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
//                    Thread.Sleep(1000);
//                    myWebClient.DownloadFile(_url, arquivoPath);
//                }

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Cria o XML da página.
//        /// </summary>
//        /// <param name="driver"></param>
//        protected void DownloadXml(/*PhantomJSDriver*/ ChromeDriver driver)
//        {
//            try
//            {
//                var certificado = ControleCertificados.FindClientCertificate("511d1904137f8ed4");

//                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

//                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop", "ExtratoXML" + horaData + ".xml");

//                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
//                {
//                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

//                    myWebClient.DownloadFileAsync(new Uri(_urlDownloadXML), arquivoPath);

//                    myWebClient.DownloadFileCompleted += MyWebClient_DownloadFileCompleted;
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Erro: " + e.Message);
//            }
//        }

//        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
//        {
//            Console.WriteLine("Download Concluído.");
//        }

//        private void DownloadXML2(/*PhantomJSDriver*/ ChromeDriver driver)
//        {
//            try
//            {
//                #region oldCode
//                //HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(_urlDownloadXML);

//                //var certificado = FindClientCertificate("511d1904137f8ed4");

//                //httpRequest.ClientCertificates.Add(certificado);

//                //httpRequest.CookieContainer = new CookieContainer();

//                //foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
//                //{
//                //    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

//                //    httpRequest.CookieContainer.Add(cookie);
//                //}

//                //using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
//                //using (Stream stream = response.GetResponseStream())
//                //using (StreamReader reader = new StreamReader(stream))
//                //{
//                //    var html = reader.ReadToEnd();

//                //    Console.WriteLine(reader.ReadToEnd());
//                //} 
//                #endregion

//                var requisicaoWeb = WebRequest.CreateHttp(_urlDownloadXML);
//                requisicaoWeb.Method = "GET";
//                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

//                foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
//                {
//                    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

//                    requisicaoWeb.CookieContainer = new CookieContainer();

//                    requisicaoWeb.CookieContainer.Add(cookie);
//                }

//                requisicaoWeb.ClientCertificates = new X509CertificateCollection();

//                requisicaoWeb.ClientCertificates.Add(ControleCertificados.FindClientCertificate("511d1904137f8ed4"));

//                using (var resposta = requisicaoWeb.GetResponse())
//                {
//                    var streamDados = resposta.GetResponseStream();
//                    StreamReader reader = new StreamReader(streamDados);
//                    object objResponse = reader.ReadToEnd();
//                    Console.WriteLine(objResponse.ToString());
//                    Console.ReadLine();
//                    streamDados.Close();
//                    resposta.Close();
//                }

//                Console.WriteLine("Download concluído!");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Erro: " + e.Message);
//            }

//        }
//    }
//}


#endregion