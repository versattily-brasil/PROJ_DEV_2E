using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using P2E.Automacao.Entidades;
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
using System.Windows.Forms;
using System.Xml;

namespace P2E.Automacao.Processos.TaxaConversaoCambio.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www35.receita.fazenda.gov.br/tabaduaneiras-web/public/pages/security/login_publico.jsf";

        private string _urlApiBase;
        private string arquivoPath = "";
        private string valorCaptcha = "";
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - TAXA DE CONVERSÃO DE CAMBIO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            Console.WriteLine("CONFIGURANDO AMBIENTE...");
            using (var client = new HttpClient())
            {
                try
                {
                    ChromeOptions options = new ChromeOptions();

                    var downloadDirectory = "C:\\Versatilly";

                    options.AddUserProfilePreference("download.default_directory", downloadDirectory);
                    options.AddUserProfilePreference("download.prompt_for_download", false);
                    options.AddUserProfilePreference("disable-popup-blocking", true);
                    options.AddArguments("--disable-web-security");
                    options.AddArguments("--allow-running-insecure-content");
                    options.AddArgument("--disable-privacy");
                    //options.AddArguments("headless");

                    options.AddArgument("test-type");
                    options.AddArgument("no-sandbox");
                    //service.HideCommandPromptWindow = true;

                    // Initialize the Chrome Driver
                    using (var _driver = new ChromeDriver(options))
                    {
                        try
                        {
                            Acessar(_driver);

                            Console.ReadKey();
                        }
                        catch (Exception)
                        {
                            _driver.Close();
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        private async Task Acessar(ChromeDriver _driver)
        {
            try
            {
                Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
                _driver.Navigate().GoToUrl(_urlSite);
                Thread.Sleep(5000);

                Console.WriteLine("PREPARANDO SITE...");
                Console.WriteLine("CAPTURANDO STRING BASE64 DA IMAGEM...");

                var aux = _driver.PageSource;
                var auxInicial = aux.IndexOf("image/png;base64,");
                var auxFinal = aux.IndexOf("divButtons_captcha_serpro_gov_br");

                var lenghtImage = ((auxFinal - 11) - (auxInicial + 17));

                var image64 = aux.Substring(auxInicial + 17, lenghtImage);   //AzCaptcha.ConverteImageParaBase64Url(_urlSite);

                Console.WriteLine("OBTENDO O VALOR DO CAPTCHA...");
                valorCaptcha = AzCaptcha.ResultadoCaptcha(image64, "yxzwck7b3wmmtx65djusgohidsfefvel");
                Console.WriteLine("CAPTCHA: " + valorCaptcha);

                Console.WriteLine("INSERINDO VALOR DO CAPTCHA NO BROWSER...");
                IWebElement element = _driver.FindElementById("txtTexto_captcha_serpro_gov_br");

                Console.WriteLine("inserindo o numero da declaração");
                element.SendKeys(valorCaptcha);

                element = _driver.FindElementByCssSelector(@"#j_id11 > div:nth-child(4) > center > input");
                element.Click();

                Thread.Sleep(5000);

                if (_driver.PageSource.Contains("Texto digitado não corresponde a imagem"))
                {
                    Console.WriteLine("Texto digitado não corresponde a imagem");
                    _driver.Close();
                    Console.ReadKey();
                }
                else
                {
                    //CLICA EM TAXA DE CONVERSAO DE CAMBIO
                    element = _driver.FindElement(By.CssSelector("#j_id110\\:agrupamento\\:13\\:grupo\\:0\\:j_id121 > center > a"));
                    element.Click();

                    Thread.Sleep(5000);

                    //CLICA EM TODOS
                    element = _driver.FindElement(By.Id(@"j_id111:j_id163"));
                    element.Click();

                    Thread.Sleep(5000);

                    //CLICA NO EXTRAIR TABELA
                    element = _driver.FindElement(By.Id(@"j_id111:j_id207"));
                    element.Click();

                    Thread.Sleep(5000);

                    element = _driver.FindElement(By.CssSelector("#j_id111\\:arquivoxml"));
                    element.Click();
                    Thread.Sleep(5000);

                    SendKeys.SendWait("{TAB}");
                    Thread.Sleep(2000);
                    SendKeys.SendWait("{TAB}");
                    Thread.Sleep(2000);
                    SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(2000);

                    var baixaXml = true;// DownloadXML(_driver);

                    if (baixaXml)
                    {
                        //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                        if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                        {
                            System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                        }

                        arquivoPath = Path.Combine("C:\\Versatilly\\TaxaConversaoCambio.xml");

                        await DeleteTaxa();

                        TaxaCambio taxaCambio = new TaxaCambio();

                        //XElement xml = XElement.Load(arquivoPath);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(arquivoPath);

                        //Pegando elemento pelo nome da TAG
                        XmlNodeList xnList = xmlDoc.GetElementsByTagName("TaxaConversaoCambio");

                        //Usando for para imprimir na tela
                        for (int i = 0; i < xnList.Count; i++)
                        {
                            string sNome = xnList[i]["codigo"].InnerText;
                            string descricao = xnList[i]["descricao"].InnerText;
                            string DataInicial = xnList[i]["inicioVigencia"].InnerText;
                            string DataFinal = xnList[i]["fimVigencia"].InnerText;
                            string Taxa = xnList[i]["taxaConversao"].InnerText;

                            Console.WriteLine("Moeda: " + sNome + " Descricao: " + descricao + "DataI: " + DataInicial + "DataF: " + DataFinal + "Taxa: " + Taxa);


                            taxaCambio.TX_MOEDA = sNome;
                            taxaCambio.TX_DESCRICAO = descricao;
                            taxaCambio.DT_INICIO_VIGENCIA = DateTime.Parse(DataInicial);
                            try { taxaCambio.DT_FIM_VIGENCIA = DateTime.Parse(DataFinal); } catch { taxaCambio.DT_FIM_VIGENCIA = null; }
                            taxaCambio.VL_TAXA_CONVERSAO = decimal.Parse(Taxa);

                            await AtualizaTaxaCambio(taxaCambio);

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro-" + e.Message.Trim());
                _driver.Close();
                Console.ReadKey();
            }
        }

        //protected bool DownloadXML(PhantomJSDriver _driver)
        //{
        //    try
        //    {
        //        var certificado = ControleCertificados.GetClientCertificate();

        //        using (var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(certificado))
        //        {
        //            var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

        //            //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
        //            if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
        //            {
        //                System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
        //            }

        //            arquivoPath = Path.Combine("C:\\Versatilly\\TaxaConversaoCambio.xml");

        //            if (!File.Exists(arquivoPath))
        //            {
        //                driver.Navigate().GoToUrl(_urlSite);

        //                var aux = driver.PageSource;
        //                var auxInicial = aux.IndexOf("image/png;base64,");
        //                var auxFinal = aux.IndexOf("divButtons_captcha_serpro_gov_br");

        //                var lenghtImage = ((auxFinal - 11) - (auxInicial + 17));

        //                var image64 = aux.Substring(auxInicial + 17, lenghtImage);   //AzCaptcha.ConverteImageParaBase64Url(_urlSite);

        //                Console.WriteLine("OBTENDO O VALOR DO CAPTCHA...");
        //                valorCaptcha = AzCaptcha.ResultadoCaptcha(image64, "yxzwck7b3wmmtx65djusgohidsfefvel");

        //                IWebElement element = driver.FindElement(By.Id("txtTexto_captcha_serpro_gov_br"));

        //                Console.WriteLine("inserindo o numero da declaração");
        //                element.SendKeys(valorCaptcha);

        //                element = driver.FindElement(By.CssSelector(@"#j_id11 > div:nth-child(4) > center > input"));
        //                element.Click();

        //                //CLICA EM TAXA DE CONVERSAO DE CAMBIO
        //                element = driver.FindElement(By.CssSelector("#j_id110\\:agrupamento\\:13\\:grupo\\:0\\:j_id121 > center > a"));
        //                element.Click();

        //                Thread.Sleep(5000);

        //                //CLICA EM TODOS
        //                element = driver.FindElement(By.Id(@"j_id111:j_id163"));
        //                element.Click();



        //                driver._my.Navigate(new Uri("https://www35.receita.fazenda.gov.br/tabaduaneiras-web/private/pages/taxaConversaoCambio_listar.jsf"),
        //                    "j_id111%3AtaxaConversaoCambioMB_tipoConsulta=0&j_id111%3AvalorConsulta_codigo=&j_id111%3ApanelDownloadArquivoOpenedState=&j_id111=j_id111&autoScroll=&javax.faces.ViewState=j_id4&j_id111%3Aj_id218=j_id111%3Aj_id218",
        //                    "application/x-www-form-urlencoded");

        //                Thread.Sleep(5000);

        //                File.WriteAllBytes(arquivoPath, ConvertToByteArray(driver.PageSource));
        //            }

        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
            return arr;
        }

        private async Task AtualizaTaxaCambio(TaxaCambio taxaConversaoCambio)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    var auxA = JsonConvert.SerializeObject(taxaConversaoCambio);
                    HttpContent httpContent = new StringContent(auxA, Encoding.UTF8, "application/json");

                    client.BaseAddress = new Uri(_urlApiBase);
                    //resultado = await client.PutAsJsonAsync($"imp/v1/taxa/{0}", taxaConversaoCambio);
                    resultado = await client.PostAsync($"imp/v1/taxa", httpContent);
                    resultado.EnsureSuccessStatusCode();

                    Console.WriteLine("Registro salvo com sucesso.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar a Taxa  {taxaConversaoCambio.TX_DESCRICAO}.");
            }
        }

        private async Task DeleteTaxa()
        {
            try
            {
                string url = _urlApiBase + $"imp/v1/taxa";

                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url + "/todos");
                    var aux = await result.Content.ReadAsStringAsync();
                    var registros = JsonConvert.DeserializeObject<List<TaxaCambio>>(aux);

                    if (registros != null && registros.Any())
                    {
                        string responseBody = string.Empty;

                        foreach (var ncm in registros)
                        {
                            result = await client.DeleteAsync($"{url}/{ncm.CD_TAXA_CAMBIO}");
                            responseBody = await result.Content.ReadAsStringAsync();
                            result.EnsureSuccessStatusCode();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao deletar tabela NCM");
            }
        }
    }
}
