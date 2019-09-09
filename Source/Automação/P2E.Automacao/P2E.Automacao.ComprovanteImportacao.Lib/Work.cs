using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.ComprovanteImportacao.Lib.Entities;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace P2E.Automacao.ComprovanteImportacao.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        private string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        private string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/RecuperarComprovanteMenu.do";
        private string _urlImprimir = @"https://www1c.siscomex.receita.fazenda.gov.br/impdespacho-web-7/ImprimirComprovante.do";

        #endregion
        public void Executar(object o)
        {
            Console.WriteLine("#################  INICIALIZANDO - COMPROVANTE DE IMPORTACAO  ################# ");
            CarregarImportacao();
        }

        public async void CarregarImportacao()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    TBImportacao tbImportacao = null;

                    Console.WriteLine("ABRE CONEXAO...");
                    client.BaseAddress = new Uri("http://localhost:7000/");
                    var result = client.GetAsync($"imp/v1/importacao/todos").Result;
                    result.EnsureSuccessStatusCode();

                    if (result.IsSuccessStatusCode)
                    {
                        var aux = await result.Content.ReadAsStringAsync();
                        var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);
                        Console.WriteLine("CARREGA DI's");
                        foreach (var item in importacao)
                        {
                            if (item.TX_NUM_DEC.Trim().Length == 10)
                            {
                                Console.WriteLine("################# DI: " + item.TX_NUM_DEC + " #################");

                                Acessar(item, item.TX_NUM_DEC);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Número da declaração está incorreto! " + e.ToString());
            }
        }

        protected void Acessar(TBImportacao import, string numero)
        {
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                Console.WriteLine("CARREGANDO CERTIFICADO...");
                // carrega o cerificado.. retirar se não for necessário.
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                var numDeclaracao = numero;

                using (var _driver = new PhantomJSDriver(service))
                {
                    Console.WriteLine("ACESSANDO SITE...");
                    _driver.Navigate().GoToUrl(_urlSite);
                   
                    Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
                    _driver.Navigate().GoToUrl(_urlTelaConsulta);
                    

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
            }
        }

        private void MyWebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download Concluído.");
        }

        protected bool DownloadComprovante(PhantomJSDriver driver, string _url)
        {
            try
            {
                var certificado = ControleCertificados.FindClientCertificate("511d19041380bd8e");

                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

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
