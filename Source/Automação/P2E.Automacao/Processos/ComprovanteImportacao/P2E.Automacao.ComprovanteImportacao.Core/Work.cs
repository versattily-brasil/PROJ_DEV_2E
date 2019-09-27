using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Versattily.WebDriver;

namespace P2E.Automacao.Processos.ComprovanteImportacao.Core
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/RecuperarComprovante.do";
        public string _urlImprimir = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/ImprimirComprovante.do";

        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion

        public Work()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            _urlApiBase = builder.GetSection("ApiBaseUrl").Value;
            _urlApiBase = "http://gateway.2e.versattily.com/";
        }

        public void Executar()
        {
            Console.WriteLine("Obtendo DI's para Download de Comprovante.");
            CarregarListaDI();
        }

        private void CarregarListaDI()
        {
            try
            {
                string urlAcompanha = _urlApiBase + $"imp/v1/importacao/todos";

                using (var client = new HttpClient())
                {
                    Console.WriteLine("ABRINDO CONEXAO...");
                    var result = client.GetStringAsync(urlAcompanha).Result;
                    registros = JsonConvert.DeserializeObject<List<Importacao>>(result);

                    if (registros != null && registros.Any())
                    {
                        using (var driver = new VersattilyDriver(ControleCertificados.FindClientCertificate("511d19041380bd8e")))
                        {
                            Console.WriteLine(_urlSite);
                            driver.Navigate().GoToUrl(_urlSite);

                            foreach (var di in registros)
                            {
                                Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

                                List<Thread> threads = new List<Thread>();

                                var thread = new Thread(() => Acessar(di.TX_NUM_DEC, driver));
                                thread.Start();
                                threads.Add(thread);

                                // fica aguardnado todas as threads terminarem...
                                while (threads.Any(t => t.IsAlive))
                                {
                                    continue;
                                }
                            }

                            Console.ReadKey();

                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO: " + e.Message);
            }
        }

        private async Task Acessar(string numero, VersattilyDriver browser)
        {
            try
            {
                Console.WriteLine("Inciando processo de navegação...");

                var numDeclaracao = numero;

                string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                                numDeclaracao.Substring(2, 7) + "-" +
                                numDeclaracao.Substring(9, 1);

                browser._my.Navigate(new Uri(_urlTelaConsulta), "fase=a&tipoComprovante=1&nrDeclaracao=%2F-&declaracoesArray=" + numeroDec,
                    "application/x-www-form-urlencoded");
                Console.WriteLine(_urlTelaConsulta);

                if (browser.PageSource.Contains("COMPROVANTE JA EMITIDO. UTILIZAR EMISSAO SEGUNDA VIA"))
                {
                    Console.WriteLine("COMPROVANTE JA EMITIDO. UTILIZAR EMISSAO SEGUNDA VIA");

                    browser._my.Navigate(new Uri(_urlTelaConsulta), "fase=a&tipoComprovante=2&nrDeclaracao=%2F-&declaracoesArray=" + numeroDec,
                    "application/x-www-form-urlencoded");
                }
                else if (browser.PageSource.Contains("DECLARACAO NAO ESTA DESEMBARACADA"))
                {
                    Console.WriteLine("DECLARACAO NAO ESTA DESEMBARACADA");
                    return;
                }

                string Numero = numDeclaracao.Substring(0, 2) + "/" +
                            numDeclaracao.Substring(2, 7) + "-" +
                            numDeclaracao.Substring(9, 1);

                string id_tr = "tr_" + Numero;

                if (browser.PageSource.Contains("COMPROVANTE RECUPERADO COM SUCESSO"))
                {
                    Console.WriteLine("COMPROVANTE RECUPERADO COM SUCESSO");

                    Console.WriteLine("DOWNLOAD DE COMPROVANTE PDF...");
                    DownloadComprovante(browser, _urlImprimir + "?nrDeclaracao=" + numeroDec, Numero);
                }
            }
            catch (Exception e)
            {
                browser.Close();
            }
        }

        protected bool DownloadComprovante(VersattilyDriver browser, string _url, string numeroDI)
        {
            try
            {
                string sNomeArquivo = numeroDI.Replace("/", "_");

                var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-COMPROVANTE_DI.pdf");

                //browser._my.Navigate(new Uri(_url), "consulta=true", "application/x-www-form-urlencoded");                

                //File.WriteAllBytes(arquivoPath, ConvertToByteArray(browser.PageSource));
                //File.WriteAllBytes(sNomeArquivo, ConvertToByteArray(browser.PageSource));

                using (WebClient myWebClient = new P2EWebClient(certificado, browser))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
                    myWebClient.Headers.Add("", "");
                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception e)
            {
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
