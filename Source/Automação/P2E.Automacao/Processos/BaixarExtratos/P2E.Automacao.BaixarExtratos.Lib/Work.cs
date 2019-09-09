using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.BaixarExtratos.Lib.Entities;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace P2E.Automacao.BaixarExtratos.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
        public string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";//?nrDeclaracao=19/0983204-0&consulta=true";
        public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do";
        private string _pathDir = @"C:\Users\Jorge.PATRIMONIO\Desktop\arquivo";
        private string _nrDeclaracao = "1909832040";
        private string _urlApiBase;
        private List<TBImportacao> registros;

        #endregion
        public Work()
        {
            Console.WriteLine("#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
           // client.BaseAddress = new Uri("http://localhost:7000/");
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
                Console.WriteLine("ABRINDO CONEXAO...");
                var result = await client.GetAsync(urlAcompanha);
                var aux = await result.Content.ReadAsStringAsync();
                var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

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
                            foreach (var di in registros)
                            {
                                Console.WriteLine("################## DI: " + di.TX_NUM_DEC + " ##################");

                                Acessar(di.TX_NUM_DEC, _driver);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver)
        {
            Console.WriteLine("Inciando processo de navegação...");

            //navega para primeira url.
            //onde é realizado o login através do certificado.
            _driver.Navigate().GoToUrl(_urlSite);
            Console.WriteLine(_driver.Url);

            //Navega para seguinda url.
            //página da consulta DI.
            _driver.Navigate().GoToUrl(_urlConsultaDI);
            Console.WriteLine(_driver.Url);

            //obtendo o campo de numero de declaração.
            IWebElement element = _driver.FindElementById("nrDeclaracao");

            //inserindo o numero da declaração.
            element.SendKeys(_nrDeclaracao);

            element = _driver.FindElement(By.Name("enviar"));

            element.Click();

            //indo para a página de consulta de declaração de importação.
            _driver.FindElement(By.Id("btnRegistrarDI")).Click();

            string numeroDec = numero.Substring(0, 2) + "%2F" +
                            numero.Substring(2, 7) + "-" +
                            numero.Substring(9, 1);

            DownloadExtrato(_driver, _urlDownloadPDF + "?nrDeclaracao=" + numeroDec);

            Console.ReadKey();
        }

        /// <summary>
        /// Verifica se o diretório existe.
        /// </summary>
        protected void AvaliarDiretorio()
        {

        }

        /// <summary>
        /// Realiza o download o arquivo de Extrato.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        protected bool DownloadExtrato(PhantomJSDriver driver, string _url)
        {
            try
            {
                var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");

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

                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cria o XML da página.
        /// </summary>
        /// <param name="driver"></param>
        protected void DownloadXml(PhantomJSDriver driver)
        {
            try
            {
                var certificado = ControleCertificados.FindClientCertificate("511d1904137f8ed4");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                string arquivoPath = Path.Combine("C:\\Users\\Jorge.PATRIMONIO\\Desktop", "ExtratoXML" + horaData + ".xml");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFileAsync(new Uri(_urlDownloadXML), arquivoPath);

                    myWebClient.DownloadFileCompleted += MyWebClient_DownloadFileCompleted;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }

        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Concluído.");
        }

        private void DownloadXML2(PhantomJSDriver driver)
        {
            try
            {
                #region oldCode
                //HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(_urlDownloadXML);

                //var certificado = FindClientCertificate("511d1904137f8ed4");

                //httpRequest.ClientCertificates.Add(certificado);

                //httpRequest.CookieContainer = new CookieContainer();

                //foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
                //{
                //    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

                //    httpRequest.CookieContainer.Add(cookie);
                //}

                //using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                //using (Stream stream = response.GetResponseStream())
                //using (StreamReader reader = new StreamReader(stream))
                //{
                //    var html = reader.ReadToEnd();

                //    Console.WriteLine(reader.ReadToEnd());
                //} 
                #endregion

                var requisicaoWeb = WebRequest.CreateHttp(_urlDownloadXML);
                requisicaoWeb.Method = "GET";
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                foreach (OpenQA.Selenium.Cookie c in driver.Manage().Cookies.AllCookies)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);

                    requisicaoWeb.CookieContainer = new CookieContainer();

                    requisicaoWeb.CookieContainer.Add(cookie);
                }

                requisicaoWeb.ClientCertificates = new X509CertificateCollection();

                requisicaoWeb.ClientCertificates.Add(ControleCertificados.FindClientCertificate("511d1904137f8ed4"));

                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();
                    Console.WriteLine(objResponse.ToString());
                    Console.ReadLine();
                    streamDados.Close();
                    resposta.Close();
                }

                Console.WriteLine("Download concluído!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }

        }
    }
}
