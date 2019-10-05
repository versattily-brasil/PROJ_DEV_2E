using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.StatusDesembaracoSefaz.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"http://online.sefaz.am.gov.br/diselada/consultadi.asp";

        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - STATUS DE DESEMBARAÇO DA SEFAZ  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para Consultar Status de Desembaraço na Sefaz.");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/status-desembaraco";

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
                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            foreach (var di in registros)
                            {
                                Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

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

                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Não existe DI's para Acompanhar Status.");
                    Console.ReadKey();
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            var numAno = numero.Substring(0,2);
            var numDeclaracao = numero.Substring(2, 7);
            var numDigito = numero.Substring(9, 1);

            Console.WriteLine("ACESSANDO SITE...");
            _driver.Navigate().GoToUrl(_urlSite);

            Console.WriteLine("Inserindo o Ano...");
            //COLOCANDO O ANO
            OpenQA.Selenium.IWebElement element = _driver.FindElementById("txtAno");
            element.SendKeys("20"+numAno);

            Console.WriteLine("Inserindo o Numero...");
            //COLOCANDO O NUMERO
            element = _driver.FindElementById("txtNumeroDI");
            element.SendKeys(numDeclaracao);

            Console.WriteLine("Inserindo o Digito Verificador...");
            //COLOCANDO O NUMERO DO DIGITO VERIFICADOR
            element = _driver.FindElementById("txtDigito");
            element.SendKeys(numDigito);

            // clica no BOTAO 'Confirmar'
            element = _driver.FindElementById("pesquisar");
            element.Click();

            Console.WriteLine("Gravando o Screenshot da Tela de Consulta...");

            var retorno = capturaImagem(_driver, numero);

            Console.WriteLine("Gravando Status...");

            import.OP_STATUS_DESEMB = retorno ? 1 : 0;
            await AtualizaStatusDesembaraco(import, nroDI);

            Console.WriteLine("Concluído !!!");
        }

        public void Screenshot(IWebDriver driver, string screenshotsPasta)
        {
            ITakesScreenshot camera = driver as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile(screenshotsPasta, ScreenshotImageFormat.Png);
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

                string arquivoPath = Path.Combine("C:\\Versatilly\\", numero + "-CapturaTela.jpg");

                Screenshot(_driver, arquivoPath);
                Thread.Sleep(500);

                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        private async Task AtualizaStatusDesembaraco(Importacao import, string cd_imp)
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
    }
}
