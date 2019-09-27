//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using P2E.Automacao.Entidades;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using P2E.Automacao.Shared.Extensions;
//using SimpleBrowser;

//using System.Runtime.Serialization.Json;

//namespace P2E.Automacao.BaixarExtratos.Core
//{
//    public class Work
//    {
//        #region Variaveis Estáticas

//        public string _urlSite = "https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
//        public string _urlConsultaDI = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";
//        public string _urlDownloadPDF = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ExtratoDI.do";//?nrDeclaracao=19/0983204-0&consulta=true";
//        //public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do?";
//        public string _urlDownloadXML = "https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDiXml.do";
//        private string _urlApiBase;
//        private List<Importacao> registros;

//        #endregion
//        public Work()
//        {
//            Console.WriteLine("#####################  INICIALIZANDO - BAIXAR EXTRATO  ##################### ");

//            var builder = new ConfigurationBuilder()
//                .SetBasePath(System.AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

//            _urlApiBase = builder.GetSection("ApiBaseUrl").Value;
//            _urlApiBase = "http://gateway.2e.versattily.com/";
//        }

//        public void Executar()
//        {
//            Console.WriteLine("Obtendo DI's para Baixar Extrato.");
//            CarregarListaDI();
//        }

//        private void CarregarListaDI()
//        {
//            try
//            {
//                string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-pdf-xml";


//                using (var client = new HttpClient())
//                {
//                    Console.WriteLine("ABRINDO CONEXAO...");
//                    var result = client.GetStringAsync(urlAcompanha).Result;
//                    registros = JsonConvert.DeserializeObject<List<Importacao>>(result);

//                    if (registros != null && registros.Any())
//                    {
//                        var browser = Shared.Extensions.Geral.CriarBrowser();

//                        Console.WriteLine(_urlSite);
//                        browser.Navigate(_urlSite);

//                        foreach (var di in registros)
//                        {
//                            Console.WriteLine("################## DI: " + di.TX_NUM_DEC + " ##################");

//                            List<Thread> threads = new List<Thread>();

//                            var thread = new Thread(async () => await Acessar(browser, di.TX_NUM_DEC, di, di.CD_IMP.ToString()));
//                            thread.Start();
//                            threads.Add(thread);

//                            // fica aguardnado todas as threads terminarem...
//                            while (threads.Any(t => t.IsAlive))
//                            {
//                                continue;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//        }

//        //private static Browser CriarBrowser()
//        //{
//        //    Console.WriteLine("CARREGANDO O CERTIFICADO...");
//        //    var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");
//        //    var browser = new Browser(certificado);
//        //    browser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";

//        //    return browser;
//        //}

//        private async Task Acessar(Browser browser, string numero, Importacao import, string cd_imp)
//        {
//            try
//            {
//                Console.WriteLine("Inciando processo de navegação...");

//                //Navega para seguinda url.
//                //página da consulta DI.
//                browser.Navigate(_urlConsultaDI);
//                Console.WriteLine(_urlConsultaDI);

//                //obtendo o campo de numero de declaração.
//                Console.WriteLine("inserindo o numero da declaração");
//                browser.Find("nrDeclaracao").Value = numero;
                
//                Console.WriteLine("Acionando o Click no enviar.");
//                Thread.Sleep(1000);

//                browser.Find(ElementType.Button, FindBy.Name, "enviar").Click();
//                Thread.Sleep(1000);

//                //indo para a página de consulta de declaração de importação.
//                browser.Find("btnRegistrarDI").Click();
//                Thread.Sleep(1000);

//                Console.WriteLine("Baixando o Extrato - PDF.");
//                Thread.Sleep(1000);

//                var gerouXml = DownloadExtratoXML(browser, numero);
//                var gerouPDF = DownloadExtratoPDF(browser, numero);

//                 //import.OP_EXTRATO_PDF = returnoPDF ? 1 : 0;

//                await AtualizaExtratoPdfXml(import, cd_imp);
//            }
//            catch (Exception e)
//            {
//                browser.Close();
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
//        /// <param name = "driver" ></ param >
//        /// < returns ></ returns >
//        protected bool DownloadExtratoPDF(Browser browser, string numero)
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

//                if (!File.Exists(arquivoPath))
//                {
//                    browser.Navigate($"{_urlDownloadPDF}?nrDeclaracao={numero}&consulta=true");
                        
//                    Thread.Sleep(5000);

//                    File.WriteAllBytes(arquivoPath, ConvertToByteArray(browser.CurrentHtml));
//                }

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        protected bool DownloadExtratoXML(Browser browser,string numero)
//        {
//            try
//            {
//                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

//                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
//                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
//                {
//                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
//                }

//                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-Extrato.xml");


//                if (!File.Exists(arquivoPath))
//                {
//                    browser.Navigate(_urlConsultaDI);
//                    browser.Navigate(new Uri(_urlDownloadXML),
//                        "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + numero + "&numeroRetificacao=&enviar=Consultar",
//                        "application/x-www-form-urlencoded");

//                    browser.Find("input", FindBy.Id, "nrDeclaracaoXml").Value = numero;
//                    browser.Find("form", FindBy.Name, "ConsultarDiXmlForm").SubmitForm();

//                    Thread.Sleep(5000);

//                    File.WriteAllBytes(arquivoPath, ConvertToByteArray(browser.CurrentHtml));
//                }

//                return true;
//            }
//            catch (Exception e)
//            {
//                return false;
//            }
//        }


//        public static byte[] ConvertToByteArray(string str)
//        {
//            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
//            return arr;
//        }

//    }
//}
