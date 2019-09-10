using Ionic.Zip;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Processos.EnviarPLI.Lib.Entidades;
using P2E.Automacao.Processos.EnviarPLI.Lib.Entidades.EstruturaPLI;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnvioPLI.Lib
{
    public class Work
    {
        #region Variaveis Estáticas
        private string _urlSite = "https://portal1.suframa.gov.br/pmerecebimento/rotinas.do";
        private string _loginSite = string.Empty;
        private string _senhaSite = string.Empty;
        private string _msgRetorno = string.Empty;
        private string _urlApiBase;
        #endregion

        public Work()
        {
            Console.WriteLine($"********************************************************************************************************************");
            Console.WriteLine("ROBÔ 12 – ENVIAR PLI");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            Console.WriteLine($"********************************************************************************************************************");
        }

        public void Executar(object o)
        {
            // Obter processos não registrados
            var registros = ObterRegistrosPendentesAsync().Result;

            if (registros != null)
            {
                foreach (var item in registros)
                {
                    // gerar e preparar arquivo
                    string nomeArquivo = GerarArquivoPLI();

                    // enviar arquivo
                    EnviarArquivo(nomeArquivo, item);

                    // atualizar registro
                    AtualizarRegistroAsync(item);
                
                }
            }
        }

        private async Task AtualizarRegistroAsync(EnvioPLIDTO item)
        {
            item.DT_DATA_EXEC = DateTime.Now;

            HttpResponseMessage resultado;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_urlApiBase);

                resultado = await client.PutAsJsonAsync($"imp/v1/enviopli/{item.CD_ENV_PLI}", item);

                resultado.EnsureSuccessStatusCode();

                Console.WriteLine("Registro salvo com sucesso.");
            }
        }

        private void EnviarArquivo(string nomeArquivo, EnvioPLIDTO envioPLIDTO)
        {
            try
            {
                using (var service = PhantomJSDriverService.CreateDefaultService())
            {
                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
                {
                        try
                        {
                            _driver.Navigate().GoToUrl(_urlSite);

                            OpenQA.Selenium.IWebElement element = _driver.FindElementById("login");

                            var credencial = ObterCredenciaisSuframa();

                            element.SendKeys(credencial.Usuario);

                            element = _driver.FindElementByName("field(-senha)");

                            element.SendKeys(credencial.Senha);

                            element = _driver.FindElementByName("btLogar");
                            element.Click();

                            // localizar ~link com o texto "Enviar PLI e efetuar o click"
                            element = _driver.FindElementByPartialLinkText("Enviar PLI");
                            element.Click();

                            // localizar o elemento File com o nome manter-arquivo
                            element = _driver.FindElementByName("field(-manter-arquivo)");

                            // seleciona o arquivo
                            element.SendKeys(nomeArquivo);

                            //localiza e clica no botão Enviar PLI
                            element = _driver.FindElementById("btnEnviarpli");
                            element.Click();

                            // Recupera a mensagem de erro caso exista.
                            try
                            {
                                element = _driver.FindElementById("ERROR");

                                if (element == null || string.IsNullOrEmpty(element.Text))
                                {
                                    envioPLIDTO.TX_LOG = "Arquivo enviado com sucesso.";
                                    envioPLIDTO.OP_STATUS = eStatus.CONCLUIDO_SEM_ERRO;
                                }
                                else
                                {
                                    envioPLIDTO.TX_LOG = element.Text;
                                    envioPLIDTO.OP_STATUS = eStatus.CONCLUIDO_COM_ERRO;
                                }
                            }
                            catch (Exception)
                            {
                                envioPLIDTO.TX_LOG = "Arquivo enviado com sucesso.";
                                envioPLIDTO.OP_STATUS = eStatus.CONCLUIDO_SEM_ERRO;
                            }
                        }
                        catch (Exception)
                        {
                            _driver.Close();
                        }
                }
            }
            }
            catch (Exception exs)
            {
                envioPLIDTO.TX_LOG = exs.Message;
                envioPLIDTO.OP_STATUS = eStatus.CONCLUIDO_COM_ERRO;
            }
        }

        private string GerarArquivoPLI()
        {
            #region Obtem dados da PLI  -  Dados Mockados
            var importador = new Importador();
            importador.tipoImportador = 2;
            importador.cdImportador = 12345678900014;

            var item1 = new ItemMatriz();
            item1.cdNcmProdFinal = 12345678;
            item1.cdSuframa = 1234;
            item1.cdDestinacao = 12;
            item1.cdUtilizacao = 34;
            item1.cdTributacao = "ABCD";
            item1.numDecreto = 888882019;
            item1.dtInicioBeneficio = 20190101;
            item1.dtFimBeneficio = 20191031;

            importador.itemMatriz = new List<ItemMatriz>();

            importador.itemMatriz.Add(item1);

            var item2 = new ItemMatriz();
            item2.cdNcmProdFinal = 12345678;
            item2.cdSuframa = 1234;
            item2.cdDestinacao = 12;
            item2.cdUtilizacao = 34;
            item2.cdTributacao = "ABCD";
            item2.numDecreto = 888882019;
            item2.dtInicioBeneficio = 20190101;
            item2.dtFimBeneficio = 20191031;

            importador.itemMatriz.Add(item2);
            #endregion

            #region gera Xml
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(importador.GetType());

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Importador));

            string path = Path.GetTempPath() + "//xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString();

            var filePath = path + "\\" + fileName + ".xml";
            using (StreamWriter file = File.AppendText(filePath))
            {
                writer.Serialize(file, importador);
                file.Close();
            }
            #endregion

            #region zipa arquivo
            var files = Directory.GetFiles(path, fileName + ".xml");

            //provide the path and name for the zip file to create
            string zipFile = path + "\\" + fileName + ".PL5ZIP";

            CriarArquivoZip(files.ToList(), zipFile);

            return zipFile;
            #endregion
        }

        private static void CriarArquivoZip(List<string> arquivos, string ArquivoDestino)
        {
            Console.WriteLine("Criando arquivo .PL5ZIP");

            using (ZipFile zip = new ZipFile())
            {
                // percorre todos os arquivos da lista
                foreach (string item in arquivos)
                {
                    // se o item é um arquivo
                    if (File.Exists(item))
                    {
                        try
                        {
                            // Adiciona o arquivo na pasta raiz dentro do arquivo zip
                            zip.AddFile(item, "");
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    // se o item é uma pasta
                    else if (Directory.Exists(item))
                    {
                        try
                        {
                            // Adiciona a pasta no arquivo zip com o nome da pasta 
                            zip.AddDirectory(item, new DirectoryInfo(item).Name);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                // Salva o arquivo zip para o destino
                try
                {
                    zip.Save(ArquivoDestino);
                }
                catch
                {
                    throw;
                }
            }
        }

        private CredenciaisSisComex ObterCredenciaisSuframa()
        {
            Console.WriteLine("Obtendo credenciais siscomex.");

            // Obter credenciais para acesso ao site
            return new CredenciaisSisComex() { Usuario = "08281892000158", Senha = "2edespachos" };
        }

        /// <summary>
        /// Obtem os registros pendentes de envio de pli.
        /// </summary>
        /// <returns></returns>
        private async Task<List<EnvioPLIDTO>> ObterRegistrosPendentesAsync()
        {
            Console.WriteLine("Obtendo registros pendentes de envio.");
            
            string urlEnvioPli = _urlApiBase + $"imp/v1/enviopli/pendentes";

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlEnvioPli);
                var lista = await result.Content.ReadAsAsync<List<EnvioPLIDTO>>();
                return lista;
            }
        }
    }
}
