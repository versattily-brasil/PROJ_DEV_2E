using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
        int _cd_bot_exec;
        ChromeOptions options = null;
        private List<TaxaCambio> registros;

        #endregion

        public Work()
        {
            LogController.RegistrarLog("#################  INICIALIZANDO - TAXA DE CONVERSÃO DE CAMBIO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work(int cd_bot_exec)
        {
            _cd_bot_exec = cd_bot_exec;
            LogController.RegistrarLog("#################  INICIALIZANDO - TAXA DE CONVERSÃO DE CAMBIO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            try
            {
                await CarregarListaDIAsync();
            }
            catch (Exception e)
            {

            }
        }

        private async Task CarregarListaDIAsync()
        {
            LogController.RegistrarLog("CONFIGURANDO AMBIENTE...", eTipoLog.INFO, _cd_bot_exec, "bot");

            try
            {
                options = new ChromeOptions();

                var downloadDirectory = "C:\\Versatilly";

                options.AddUserProfilePreference("download.default_directory", downloadDirectory);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                options.AddUserProfilePreference("safebrowsing.enabled", "false");

                //options.AddArguments("headless");
                //options.AddArgument("--start-maximized");
                options.AddArgument("test-type");
                options.AddArgument("no-sandbox");
                options.AddArgument(Directory.GetCurrentDirectory() + @"\chromedriver.exe");
                LogController.RegistrarLog(Directory.GetCurrentDirectory() + @"\chromedriver.exe", eTipoLog.INFO, _cd_bot_exec, "bot");

                // Initialize the Chrome Driver
                using (var _driver = new ChromeDriver(options))
                {
                    try
                    {
                        Acessar(_driver);
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

        private async Task Acessar(ChromeDriver _driver)
        {
            try
            {
                var validaCaptcha = false;
                IWebElement element = null;

                while (!validaCaptcha)
                {
                    LogController.RegistrarLog("ACESSAO PAGINA DE CONSULTA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    _driver.Navigate().GoToUrl(_urlSite);
                    Thread.Sleep(5000);

                    LogController.RegistrarLog("PREPARANDO SITE...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    LogController.RegistrarLog("CAPTURANDO STRING BASE64 DA IMAGEM...", eTipoLog.INFO, _cd_bot_exec, "bot");

                    var aux = _driver.PageSource;
                    var auxInicial = aux.IndexOf("image/png;base64,");
                    var auxFinal = aux.IndexOf("divButtons_captcha_serpro_gov_br");

                    var lenghtImage = ((auxFinal - 11) - (auxInicial + 17));

                    var image64 = aux.Substring(auxInicial + 17, lenghtImage);   //AzCaptcha.ConverteImageParaBase64Url(_urlSite);

                    LogController.RegistrarLog("OBTENDO O VALOR DO CAPTCHA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    valorCaptcha = AzCaptcha.ResultadoCaptcha(image64, "yxzwck7b3wmmtx65djusgohidsfefvel");
                    LogController.RegistrarLog("CAPTCHA: " + valorCaptcha);

                    LogController.RegistrarLog("INSERINDO VALOR DO CAPTCHA NO BROWSER...", eTipoLog.INFO, _cd_bot_exec, "bot");
                    element = _driver.FindElementById("txtTexto_captcha_serpro_gov_br");

                    element.SendKeys(valorCaptcha);

                    element = _driver.FindElementByCssSelector(@"#j_id11 > div:nth-child(4) > center > input");
                    element.Click();

                    Thread.Sleep(3000);

                    if (_driver.PageSource.Contains("Texto digitado não corresponde a imagem"))
                    {
                        LogController.RegistrarLog("Texto digitado não corresponde a imagem", eTipoLog.INFO, _cd_bot_exec, "bot");
                        validaCaptcha = false;
                    }
                    else
                    {
                        validaCaptcha = true;
                    }
                }

                File.Delete("C:\\Versatilly\\TaxaConversaoCambio.xml");

                //CLICA EM TAXA DE CONVERSAO DE CAMBIO
                LogController.RegistrarLog("CLICA EM TAXA DE CONVERSAO DE CAMBIO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElement(By.CssSelector("#j_id110\\:agrupamento\\:13\\:grupo\\:0\\:j_id121 > center > a"));
                element.Click();

                Thread.Sleep(3000);

                //CLICA EM TODOS
                LogController.RegistrarLog("CLICA EM TODOS...", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElement(By.Id(@"j_id111:j_id163"));
                element.Click();

                Thread.Sleep(2000);

                //CLICA NO EXTRAIR TABELA
                LogController.RegistrarLog("CLICA NO EXTRAIR TABELA...", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElement(By.Id(@"j_id111:j_id207"));
                element.Click();

                Thread.Sleep(1000);

                LogController.RegistrarLog("CLICA NO XML PARA DOWNLOAD...", eTipoLog.INFO, _cd_bot_exec, "bot");
                element = _driver.FindElement(By.CssSelector("#j_id111\\:arquivoxml"));
                element.Click();
                Thread.Sleep(5000);

                LogController.RegistrarLog("FECHANDO O NAVEGADOR...", eTipoLog.INFO, _cd_bot_exec, "bot");
                

                LogController.RegistrarLog("NAVEGADOR FECHADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                _driver.Quit();

                var baixaXml = true;// DownloadXML(_driver);

                if (baixaXml)
                {
                    //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                    if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                    {
                        System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                    }

                    arquivoPath = Path.Combine("C:\\Versatilly\\TaxaConversaoCambio.xml");

                    try
                    {
                        // var aux = DeleteTaxa();
                    }
                    catch (Exception e)
                    {

                    }

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

                        LogController.RegistrarLog("Moeda: " + sNome + " Descricao: " + descricao + "DataI: " + DataInicial + "DataF: " + DataFinal + "Taxa: " + Taxa, eTipoLog.INFO, _cd_bot_exec, "bot");

                        //LogController.RegistrarLog("INSERINDO NA TABELA...", eTipoLog.INFO, _cd_bot_exec, "bot");

                        taxaCambio.TX_MOEDA = sNome;
                        //LogController.RegistrarLog("MOEDA -" + sNome, eTipoLog.INFO, _cd_bot_exec, "bot");
                        taxaCambio.TX_DESCRICAO = descricao;
                        //LogController.RegistrarLog("DESCRICAO - " + descricao, eTipoLog.INFO, _cd_bot_exec, "bot");
                        taxaCambio.DT_INICIO_VIGENCIA = DateTime.ParseExact(DataInicial, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//DateTime.Parse(DataInicial);
                        //LogController.RegistrarLog("DATA INICIAL - " + DataInicial, eTipoLog.INFO, _cd_bot_exec, "bot");

                        if (DataFinal.Trim() == "-")
                        {
                            //LogController.RegistrarLog("DATA FINAL - " + ".....", eTipoLog.INFO, _cd_bot_exec, "bot");
                            taxaCambio.DT_FIM_VIGENCIA = null;
                        }
                        else
                        {
                            //LogController.RegistrarLog("DATA INICIAL - " + DataFinal, eTipoLog.INFO, _cd_bot_exec, "bot");
                            taxaCambio.DT_FIM_VIGENCIA = DateTime.ParseExact(DataFinal, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //LogController.RegistrarLog("TAXA - " + Taxa, eTipoLog.INFO, _cd_bot_exec, "bot");
                        taxaCambio.VL_TAXA_CONVERSAO =  decimal.Parse(Taxa);


                        //LogController.RegistrarLog("ENVIANDO PARA O METODO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        await AtualizaTaxaCambio(taxaCambio);

                    }
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog("Erro-" + e.Message.Trim(), eTipoLog.INFO, _cd_bot_exec, "bot");
                //_driver.Close();
                ////Console.ReadKey();
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
        //            if (!System.IO.Directory.Exists(@"C:\Versatilly\"+ _nome_cliente + "\\"))
        //            {
        //                System.IO.Directory.CreateDirectory(@"C:\Versatilly\"+ _nome_cliente + "\\");
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

        //                LogController.RegistrarLog("OBTENDO O VALOR DO CAPTCHA...");
        //                valorCaptcha = AzCaptcha.ResultadoCaptcha(image64, "yxzwck7b3wmmtx65djusgohidsfefvel");

        //                IWebElement element = driver.FindElement(By.Id("txtTexto_captcha_serpro_gov_br"));

        //                LogController.RegistrarLog("inserindo o numero da declaração");
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
                var criaTaxaCambio = true;

                using (var clientTaxa = new HttpClient())
                {
                    string urlTaxa = _urlApiBase + $"imp/v1/taxa/moeda/" + taxaConversaoCambio.TX_MOEDA;

                    var result = clientTaxa.GetAsync(urlTaxa).Result;
                    var aux = await result.Content.ReadAsStringAsync();
                    registros = JsonConvert.DeserializeObject<List<TaxaCambio>>(aux);

                    if (registros != null && registros.Any())
                    {
                        foreach (var item in registros)
                        {
                            if (taxaConversaoCambio.DT_INICIO_VIGENCIA <= item.DT_INICIO_VIGENCIA)
                            {
                                criaTaxaCambio = false;
                            }
                        }
                    }
                }

                using (var client = new HttpClient())
                {
                    if (criaTaxaCambio)
                    {
                        LogController.RegistrarLog("SERIALIZANDO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        var auxA = JsonConvert.SerializeObject(taxaConversaoCambio);
                        HttpContent httpContent = new StringContent(auxA, Encoding.UTF8, "application/json");

                        LogController.RegistrarLog("ENVIANDO PARA API...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        client.BaseAddress = new Uri(_urlApiBase);

                        var resultado = client.PostAsync($"imp/v1/taxa", httpContent).Result;
                        resultado.EnsureSuccessStatusCode();

                        LogController.RegistrarLog("Registro salvo com sucesso.", eTipoLog.INFO, _cd_bot_exec, "bot");
                    }
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog($"Erro ao atualizar a Taxa  {taxaConversaoCambio.TX_DESCRICAO}.", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
        }

        private async Task DeleteTaxa()
        {
            try
            {
                string url = _urlApiBase + $"imp/v1/taxa/deleteall";

                using (var client = new HttpClient())
                {
                    string responseBody = string.Empty;

                    LogController.RegistrarLog("Excluindo Registros", eTipoLog.INFO, _cd_bot_exec, "bot");

                    var result = client.DeleteAsync($"{url}").Result;
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog($"Erro ao deletar tabela Taxa de Cambio", eTipoLog.INFO, _cd_bot_exec, "bot");
            }
        }
    }
}
