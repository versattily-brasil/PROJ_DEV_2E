using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.SqlScriptPublish;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Versattily.WebDriver;

namespace P2E.Automacao.Processos.StatusDesembaracoSefaz.Core
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"http://online.sefaz.am.gov.br/diselada/DI_Selada_oracle.asp";

        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - STATUS DE DESEMBARAÇO DA SEFAZ  ################# ");
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            _urlApiBase = builder.GetSection("ApiBaseUrl").Value;
            _urlApiBase = "http://gateway.2e.versattily.com/";
        }

        public void Executar()
        {
            Console.WriteLine("Obtendo DI's para Consultar Status de Desembaraço na Sefaz.");
            CarregarListaDIAsync();
        }

        private void CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/todos";

            using (var client = new HttpClient())
            {
                Console.WriteLine("ABRINDO CONEXAO...");
                var result = client.GetStringAsync(urlAcompanha).Result;
                registros = JsonConvert.DeserializeObject<List<Importacao>>(result);

                if (registros != null && registros.Any())
                {
                    using (var browser = new VersattilyDriver(ControleCertificados.FindClientCertificate("511d19041380bd8e")))
                    {
                        foreach (var di in registros)
                        {
                            Console.WriteLine("################# DI: " + di.TX_NUM_DEC + " #################");

                            List<Thread> threads = new List<Thread>();

                            var thread = new Thread(() => Acessar(di.TX_NUM_DEC, browser, di, di.CD_IMP.ToString()));
                            thread.Start();
                            threads.Add(thread);

                            // fica aguardnado todas as threads terminarem...
                            while (threads.Any(t => t.IsAlive))
                            {
                                continue;
                            }
                        }
                        Console.WriteLine("Robô Concluido !");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Não existe DI's para Acompanhar Status.");
                }
            }
        }

        private async Task Acessar(string numDeclaracao, VersattilyDriver browser, Importacao import, string nroDI)
        {
            Console.WriteLine("Inciando processo de navegação...");

            string ano = numDeclaracao.Substring(0, 2);
            string declaracao = numDeclaracao.Substring(2, 7);
            string digito = numDeclaracao.Substring(9, 1);

            browser._my.Navigate(new Uri(_urlSite), "primeiroAcesso=N&txtAno=20" + ano + "&txtNumeroDI=" + declaracao + "&txtDigito=" + digito + "&pesquisar=Consultar",
                "application/x-www-form-urlencoded");
            Console.WriteLine(_urlSite);

            Console.WriteLine("Gravando o Screenshot da Tela de Consulta...");

            var retorno = capturaImagem(browser._my, numDeclaracao);

            Console.WriteLine("Gravando Status...");

            import.OP_STATUS_DESEMB = retorno ? 1 : 0;
            await AtualizaStatusDesembaraco(import, nroDI);

            Console.WriteLine("Concluído !!!");
        }

        public void Screenshot(IBrowser driver, string screenshotsPasta)
        {
            ITakesScreenshot camera = driver.Browsers as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile(screenshotsPasta, ScreenshotImageFormat.Png);
        }

        public bool capturaImagem(IBrowser _driver, string numero)
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
            catch (Exception e)
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
