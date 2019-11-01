using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;

        #endregion
        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - ", eTipoLog.INFO, _cd_bot_exec, "bot");

            LogController.RegistrarLog(_nome_cliente + " - " + "#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;


            LogController.RegistrarLog(_nome_cliente + " - " + "#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ", eTipoLog.INFO, _cd_bot_exec, "bot");

            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

        }

        public async Task ExecutarAsync()
        {
            try
            {

                LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para Baixar Extrato.", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-pdf-xml/" + _cd_par;

                using (var client = new HttpClient())
                {

                    LogController.RegistrarLog(_nome_cliente + " - " + "ABRINDO CONEXÃO...", eTipoLog.INFO, _cd_bot_exec, "bot");

                    var result = await client.GetAsync(urlAcompanha);
                    registros = await result.Content.ReadAsAsync<List<Importacao>>();

                    if (registros != null && registros.Any())
                    {
                        using (var service = PhantomJSDriverService.CreateDefaultService())
                        {
                            LogController.RegistrarLog(_nome_cliente + " - " + "CARREGANDO O CERTIFICADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                            string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\client-certificate.crt";

                            LogController.RegistrarLog(_nome_cliente + " - " + cert, eTipoLog.INFO, _cd_bot_exec, "bot");
                            ControleCertificados.CarregarCertificado(service);

                            service.AddArgument("test-type");
                            service.AddArgument("no-sandbox");
                            service.HideCommandPromptWindow = true;

                            var options = new PhantomJSOptions();

                            using (var _driver = new PhantomJSDriver(service, options, TimeSpan.FromMinutes(2)))
                            {
                                try
                                {
                                    _driver.Navigate().GoToUrl(_urlSite);
                                    LogController.RegistrarLog(_nome_cliente + " - " + _driver.Url, eTipoLog.INFO, _cd_bot_exec, "bot");

                                    foreach (var di in registros)
                                    {

                                        LogController.RegistrarLog(_nome_cliente + " - " + "################## DI: " + di.TX_NUM_DEC + " ##################", eTipoLog.INFO, _cd_bot_exec, "bot");

                                        List<Thread> threads = new List<Thread>();

                                        if (di.OP_EXTRATO_XML == 0 || di.OP_EXTRATO_PDF == 0)
                                        {
                                            var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver, di, di.CD_IMP.ToString()));
                                            thread.Start();
                                            threads.Add(thread);
                                        }

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

                        LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's para Acompanhar Despacho.", eTipoLog.INFO, _cd_bot_exec, "bot");
                        ////Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {

                LogController.RegistrarLog(_nome_cliente + " - " + ex.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string cd_imp)
        {
            try
            {

                LogController.RegistrarLog(_nome_cliente + " - " + "Inciando processo de navegação...", eTipoLog.INFO, _cd_bot_exec, "bot");

                //página da consulta DI.
                _driver.Navigate().GoToUrl(_urlConsultaDI);
                LogController.RegistrarLog(_nome_cliente + " - " + _driver.Url, eTipoLog.INFO, _cd_bot_exec, "bot");

                //obtendo o campo de numero de declaração.
                IWebElement element = _driver.FindElementById("nrDeclaracao");

                LogController.RegistrarLog(_nome_cliente + " - " + "inserindo o numero da declaração - Nro.: " + numero, eTipoLog.INFO, _cd_bot_exec, "bot");

                //string di = numero.Substring(0, 10);
                element.SendKeys(numero);

                LogController.RegistrarLog(_nome_cliente + " - " + "Acionando o Click no enviar.", eTipoLog.INFO, _cd_bot_exec, "bot");
                Thread.Sleep(2000);

                element = _driver.FindElement(By.Name("enviar"));
                Thread.Sleep(2000);
                element.Click();

                Thread.Sleep(5000);

                //indo para a página de consulta de declaração de importação.
                //_driver.FindElement(By.Id("btnRegistrarDI")).Click();

                //Thread.Sleep(3000);

                Thread.Sleep(2000);

                var numeroDec = numero.Substring(0, 2) + "%2F" +
                                numero.Substring(2, 7) + "-" +
                                numero.Substring(9, 1);

                if (import.OP_EXTRATO_PDF == 0)
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + "Baixando o Extrato - PDF.", eTipoLog.INFO, _cd_bot_exec, "bot");

                    var retornoPDF = DownloadExtratoPDF(_driver, _urlDownloadPDF + "?nrDeclaracao=" + numeroDec);

                    import.OP_EXTRATO_PDF = retornoPDF ? 1 : 0;
                }

                if (import.OP_EXTRATO_XML == 0)
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + "Baixando o Extrato - XML.", eTipoLog.INFO, _cd_bot_exec, "bot");

                    var retornoXML = DownloadExtratoXML(numeroDec);

                    import.OP_EXTRATO_XML = retornoXML ? 1 : 0;
                }

                await AtualizaExtratoPdfXml(import, cd_imp);
            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "ERRO: " + ex.Message, eTipoLog.ERRO, _cd_bot_exec, "bot");
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

                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");

                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.", eTipoLog.ERRO, _cd_bot_exec, "bot");

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
                var tentarNovamente = true;
                var tentativas = 1;
                string arquivoPath = "";

                while (tentarNovamente)
                {
                    var certificado = ControleCertificados.GetClientCertificate(); ;

                    var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                    //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                    if (!System.IO.Directory.Exists(@"C:\Versatilly\" + _nome_cliente + "\\"))
                    {
                        System.IO.Directory.CreateDirectory(@"C:\Versatilly\" + _nome_cliente + "\\");
                    }

                    arquivoPath = Path.Combine(@"C:\Versatilly\" + _nome_cliente + "\\", horaData + "-Extrato.pdf");

                    using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                    {
                        myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
                        myWebClient.DownloadFile(_url, arquivoPath);

                        Thread.Sleep(5000);
                    }

                    FileInfo fileInfox = new FileInfo(arquivoPath);
                    var tamx = fileInfox.Length;
                    if (fileInfox.Length > 0)
                    {
                        tentarNovamente = false;
                    }
                    else
                    {
                        if (tentativas <= 5)
                        {
                            LogController.RegistrarLog(_nome_cliente + " - " + tentativas + "º Tentativa de Baixar o Extrato - PDF.", eTipoLog.INFO, _cd_bot_exec, "bot");
                            tentativas++;
                        }
                        else
                        {
                            tentarNovamente = false;
                        }
                    }
                }

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {

                LogController.RegistrarLog(_nome_cliente + " - " + "Erro ao Baixar Extrato PDF " + e.Message.Trim(), eTipoLog.INFO, _cd_bot_exec, "bot");
                return false;
            }
        }


        protected bool DownloadExtratoXML(string numero)
        {
            try
            {
                var tentarNovamente = true;
                var tentativas = 1;
                string arquivoPath = "";

                while (tentarNovamente)
                {
                    numero = numero.Replace("/", "");
                    numero = numero.Replace("-", "");
                    numero = numero.Replace("%2F", "");

                    var certificado = ControleCertificados.GetClientCertificate();
                    using (var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(certificado))
                    {
                        var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                        //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                        if (!System.IO.Directory.Exists(@"C:\Versatilly\" + _nome_cliente + "\\"))
                        {
                            System.IO.Directory.CreateDirectory(@"C:\Versatilly\" + _nome_cliente + "\\");
                        }

                        arquivoPath = Path.Combine(@"C:\Versatilly\" + _nome_cliente + "\\", horaData + "-Extrato.xml");


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
                    }

                    FileInfo fileInfox = new FileInfo(arquivoPath);
                    var tamx = fileInfox.Length;
                    if (fileInfox.Length > 0)
                    {
                        tentarNovamente = false;
                    }
                    else
                    {
                        if (tentativas <= 5)
                        {
                            LogController.RegistrarLog(_nome_cliente + " - " + tentativas + "º Tentativa de Baixar o Extrato - XML.", eTipoLog.INFO, _cd_bot_exec, "bot");
                            tentativas++;
                        }
                        else
                        {
                            tentarNovamente = false;
                        }
                    }
                }

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);

                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "Erro ao Baixar Extrato XML " + e.Message.Trim(), eTipoLog.INFO, _cd_bot_exec, "bot");

                return false;
            }
        }
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
//            LogController.RegistrarLog(_nome_cliente + " - " + "#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ");
//            //_urlApiBase = "http://localhost:7000/";
//            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];

//        }

//        public async Task ExecutarAsync()
//        {
//            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para Baixar Extrato.");
//            await CarregarListaDIAsync();
//        }

//        private async Task CarregarListaDIAsync()
//        {
//            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-pdf-xml";

//            using (var client = new HttpClient())
//            {
//                LogController.RegistrarLog(_nome_cliente + " - " + "ABRINDO CONEXAO...");
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

//                            LogController.RegistrarLog(_nome_cliente + " - " + "CARREGANDO O CERTIFICADO...");
//                            ControleCertificados.CarregarCertificado(options);

//                            options.AddArgument("test-type");
//                            options.AddArgument("no-sandbox");
//                            service.HideCommandPromptWindow = true;

//                            using (var _driver = new /*PhantomJSDriver*/ ChromeDriver(service, options))
//                            {
//                                _driver.Navigate().GoToUrl(_urlSite);
//                                LogController.RegistrarLog(_nome_cliente + " - " + _driver.Url);

//                                foreach (var di in registros)
//                                {
//                                    LogController.RegistrarLog(_nome_cliente + " - " + "################## DI: " + di.TX_NUM_DEC + " ##################");

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
//                        LogController.RegistrarLog(_nome_cliente + " - " + "erro no driver " + e.Message.Trim());
//                    }
//                }
//                else
//                {
//                    LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's para Acompanhar Despacho.");
//                }
//            }
//        }

//        private async Task Acessar(string numero, /*PhantomJSDriver*/ ChromeDriver _driver, Importacao import, string cd_imp)
//        {
//            try
//            {
//                LogController.RegistrarLog(_nome_cliente + " - " + "Inciando processo de navegação...");

//                ////navega para primeira url.
//                ////onde é realizado o login através do certificado.
//                //_driver.Navigate().GoToUrl(_urlSite);
//                //LogController.RegistrarLog(_nome_cliente + " - " + _driver.Url);

//                //Navega para seguinda url.
//                //página da consulta DI.
//                _driver.Navigate().GoToUrl(_urlConsultaDI);
//                LogController.RegistrarLog(_nome_cliente + " - " + _driver.Url);
//                Thread.Sleep(2000);
//                //obtendo o campo de numero de declaração.
//                IWebElement element = _driver.FindElementById("nrDeclaracao");

//                LogController.RegistrarLog(_nome_cliente + " - " + "inserindo o numero da declaração");
//                element.SendKeys(numero);

//                LogController.RegistrarLog(_nome_cliente + " - " + "Acionando o Click no enviar.");
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

//                //LogController.RegistrarLog(_nome_cliente + " - " + "Baixando o Extrato - PDF.");
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
//                //    LogController.RegistrarLog(_nome_cliente + " - " + e.ToString());
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

//                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro salvo com sucesso.");
//                }
//            }
//            catch (Exception e)
//            {
//                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.");
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
//                if (!System.IO.Directory.Exists(@"C:\Versatilly\"+ _nome_cliente + "\\"))
//                {
//                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\"+ _nome_cliente + "\\");
//                }

//                string arquivoPath = Path.Combine(@"C:\Versatilly\"+ _nome_cliente + "\\", horaData + "-Extrato.pdf");

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
//                if (!System.IO.Directory.Exists(@"C:\Versatilly\"+ _nome_cliente + "\\"))
//                {
//                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\"+ _nome_cliente + "\\");
//                }

//                string arquivoPath = Path.Combine(@"C:\Versatilly\"+ _nome_cliente + "\\", horaData + "-Extrato.xml");

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
//                LogController.RegistrarLog(_nome_cliente + " - " + "Erro: " + e.Message);
//            }
//        }

//        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
//        {
//            LogController.RegistrarLog(_nome_cliente + " - " + "Download Concluído.");
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

//                //    LogController.RegistrarLog(_nome_cliente + " - " + reader.ReadToEnd());
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
//                    LogController.RegistrarLog(_nome_cliente + " - " + objResponse.ToString());
//                    Console.ReadLine();
//                    streamDados.Close();
//                    resposta.Close();
//                }

//                LogController.RegistrarLog(_nome_cliente + " - " + "Download concluído!");
//            }
//            catch (Exception e)
//            {
//                LogController.RegistrarLog(_nome_cliente + " - " + "Erro: " + e.Message);
//            }

//        }
//    }
//}


#endregion