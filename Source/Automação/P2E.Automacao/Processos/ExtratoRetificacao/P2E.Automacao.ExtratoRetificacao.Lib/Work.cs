using Newtonsoft.Json;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.ExtratoRetificacao.Lib.Entities;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.ExtratoRetificacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarExtratoRetificacaoMenu.do";
        public string _urlDownload = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarExtratoRetificacao.do";

        private string _urlApiBase;
        private List<TBImportacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - EXTRATO DE RETIFICAÇÃO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para Consultar Extrato de Retificação.");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/extrato-retif";

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
                            Console.WriteLine("ACESSANDO SITE...");
                            _driver.Navigate().GoToUrl(_urlSite);

                            foreach (var di in registros)
                            {
                                Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

                                List<Thread> threads = new List<Thread>();

                                var thread = new Thread(() => Acessar(di.TX_NUM_DEC, _driver, di,di.CD_IMP.ToString()));
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

        private async Task Acessar(string numero, PhantomJSDriver _driver, TBImportacao import, string nroDI)
        {
            //using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))

            var numDeclaracao = numero;

            Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
            _driver.Navigate().GoToUrl(_urlTelaConsulta);

            //COLOCANDO O NUMERO DA DI NO CAMPO
            OpenQA.Selenium.IWebElement element = _driver.FindElementById("nrDeclaracao");
            element.SendKeys(numDeclaracao);

            // clica no BOTAO 'Confirmar'
            element = _driver.FindElementByCssSelector("#corpo > input:nth-child(3)");
            element.Click();

            //VERIFICA SE TEM POPUP.. PRECISA MELHORAR...
            try { var aux = element.Text; }
            catch (Exception)
            {
                import.OP_EXTRATO_RETIF = 0;

                await AtualizaExtratoRetificacao(import, nroDI); 

                Console.WriteLine("DI NÃO RETIFICADA...");
                return;
            }

            string numeroDec = numDeclaracao.Substring(0, 2) + "%2F" +
                                        numDeclaracao.Substring(2, 7) + "-" +
                                        numDeclaracao.Substring(9, 1);

            Console.WriteLine("DOWNLOAD DO EXTRATO DE RETIFICACAO EM PDF...");
            var retornoRetif = DownloadComprovante(_driver, _urlDownload + "?nrDeclaracao=" + numeroDec);

            import.OP_EXTRATO_RETIF = retornoRetif ? 1 : 0;

            await AtualizaExtratoRetificacao(import, nroDI);
        }

        private async Task AtualizaExtratoRetificacao(TBImportacao import, string cd_imp)
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

                string arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-ExtratoRetificacao.pdf");

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
