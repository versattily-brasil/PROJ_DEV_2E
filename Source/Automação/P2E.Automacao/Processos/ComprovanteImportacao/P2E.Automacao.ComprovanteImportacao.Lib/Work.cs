using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.ComprovanteImportacao.Lib;
using P2E.Automacao.ComprovanteImportacao.Lib.Entities;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.ComprovanteImportacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/RecuperarComprovanteMenu.do";
        public string _urlImprimir = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/ImprimirComprovante.do";

        private string _urlApiBase;
        private List<TBImportacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - COMPROVANTE DE IMPORTACAO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para Download de Comprovante.");
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
                registros = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

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
                                Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

                                List<Thread> threads = new List<Thread>();

                                var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver));
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
                }
                else
                {
                    Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver)
        {
            //using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))

            var numDeclaracao = numero;

            Console.WriteLine("ACESSANDO SITE...");
            _driver.Navigate().GoToUrl(_urlSite);

            Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
            _driver.Navigate().GoToUrl(_urlTelaConsulta);

            //COLOCANDO O NUMERO DA DI NO CAMPO
            OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
            element.SendKeys(numDeclaracao);

            // clica no BOTAO 'Confirmar'
            element = _driver.FindElementById("confirmar");
            element.Click();

            string Numero = numDeclaracao.Substring(0, 2) + "/" +
                            numDeclaracao.Substring(2, 7) + "-" +
                            numDeclaracao.Substring(9, 1);

            string id_tr = "tr_" + Numero;
            element = _driver.FindElementById(id_tr);

            //CAPTURA STATUS
            var status = element.Text;

            if (status.Contains("COMPROVANTE JA EMITIDO. UTILIZAR EMISSAO SEGUNDA VIA"))
            {
                Console.WriteLine(status);
                // clica no botao OK
                element = _driver.FindElementByCssSelector(@"#botoes > input");
                element.Click();

                // clica no radiobutton 2º via
                element = _driver.FindElementByCssSelector(@"#corpo > fieldset:nth-child(1) > div > input[type=radio]:nth-child(2)");
                element.Click();

                element = _driver.FindElementById("nrDeclaracao");
                element.SendKeys(numDeclaracao);

                // clica no BOTAO 'Confirmar'
                element = _driver.FindElementById("confirmar");
                element.Click();
            }
            else if (status.Contains("DECLARACAO NAO ESTA DESEMBARACADA."))
            {
                Console.WriteLine(status);
                return;
            }

            id_tr = "tr_" + Numero;

            element = _driver.FindElementById(id_tr);
            status = element.Text;

            if (status.Contains("COMPROVANTE RECUPERADO COM SUCESSO"))
            {
                Console.WriteLine(status);

                string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                            numDeclaracao.Substring(2, 7) + "-" +
                            numDeclaracao.Substring(9, 1);

                Console.WriteLine("DOWNLOAD DE COMPROVANTE PDF...");
                DownloadComprovante(_driver, _urlImprimir + "?nrDeclaracao=" + numeroDec);
            }
        }

        protected bool DownloadComprovante(PhantomJSDriver driver, string _url)
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

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-COMPROVANTE_DI.pdf");

                using (WebClient myWebClient = new P2EWebClient(certificado, driver))
                {
                    myWebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    myWebClient.DownloadFile(_url, arquivoPath);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
