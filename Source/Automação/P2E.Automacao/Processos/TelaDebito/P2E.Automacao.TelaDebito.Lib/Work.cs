using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace P2E.Automacao.Processos.TelaDebito.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";

        private string _urlApiBase;
        private List<Importacao> registros;
        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;
        #endregion

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - TELA DE DÉBITO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - TELA DE DÉBITO  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's para Consultar Tela de Débito.", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            string urlAcompanha = _urlApiBase + $"imp/v1/importacao/tela-debito/" + _cd_par;

            using (var client = new HttpClient())
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "ABRINDO CONEXAO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                var result = client.GetAsync(urlAcompanha).Result;
                var aux = await result.Content.ReadAsStringAsync();
                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

                if (registros != null && registros.Any())
                {
                    using (var service = PhantomJSDriverService.CreateDefaultService())
                    {
                        LogController.RegistrarLog(_nome_cliente + " - " + "CARREGANDO O CERTIFICADO...", eTipoLog.INFO, _cd_bot_exec, "bot");
                        ControleCertificados.CarregarCertificado(service);

                        service.AddArgument("test-type");
                        service.AddArgument("no-sandbox");
                        service.HideCommandPromptWindow = true;

                        using (var _driver = new PhantomJSDriver(service))
                        {
                            try
                            {
                                LogController.RegistrarLog(_nome_cliente + " - " + "ACESSANDO SITE...", eTipoLog.INFO, _cd_bot_exec, "bot");
                                _driver.Navigate().GoToUrl(_urlSite);

                                foreach (var di in registros)
                                {
                                    LogController.RegistrarLog(_nome_cliente + " - " + "################# DI: " + di.TX_NUM_DEC + " #################", eTipoLog.INFO, _cd_bot_exec, "bot");

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

                                LogController.RegistrarLog(_nome_cliente + " - " + "Robô Finalizado !", eTipoLog.INFO, _cd_bot_exec, "bot");
                                //Console.ReadKey();
                            }
                            catch (Exception)
                            {
                                _driver.Close();
                            }
                        }
                    }
                }
                else
                {
                    LogController.RegistrarLog(_nome_cliente + " - " + "Não existe DI's para Acompanhar Despacho.", eTipoLog.INFO, _cd_bot_exec, "bot");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            var numDeclaracao = numero;

            LogController.RegistrarLog(_nome_cliente + " - " + "ACESSAO PAGINA DE CONSULTA...", eTipoLog.INFO, _cd_bot_exec, "bot");
            _driver.Navigate().GoToUrl(_urlTelaConsulta);

            //obtendo o campo de numero de declaração.
            IWebElement element = _driver.FindElementById("nrDeclaracao");

            LogController.RegistrarLog(_nome_cliente + " - " + "inserindo o numero da declaração", eTipoLog.INFO, _cd_bot_exec, "bot");
            element.SendKeys(numero);
          
            element = _driver.FindElement(By.Name("enviar"));
            element.Click();
            Thread.Sleep(2000);

            LogController.RegistrarLog(_nome_cliente + " - " + "Criando Arquivo.....", eTipoLog.INFO, _cd_bot_exec, "bot");

            var retornFile = CreateDocumentWord(element, _driver, numero);

            LogController.RegistrarLog(_nome_cliente + " - " + "Gravando Status...", eTipoLog.INFO, _cd_bot_exec, "bot");

            import.OP_TELA_DEBITO = retornFile ? 1 : 0;

            await AtualizaTelaDebito(import, nroDI);

            LogController.RegistrarLog(_nome_cliente + " - " + "Arquivo Criado...", eTipoLog.INFO, _cd_bot_exec, "bot");
        }

        public bool CreateDocumentWord(IWebElement element, PhantomJSDriver _driver,string numero)
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"+ _nome_cliente + "\\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\"+ _nome_cliente + "\\");
                }

                string arquivoPath = Path.Combine(@"C:\Versatilly\"+ _nome_cliente + "\\", numero + "-TelaDebito.docx");

                var doc = DocX.Create(arquivoPath);

                //StreamWriter writer = new StreamWriter(arquivoPath);

                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr > td.tituloDI");
                doc.InsertParagraph(element.Text);
                doc.InsertParagraph(" ");
                doc.InsertParagraph(" ");

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(1) > td.colunaSemBorda > label");
                var auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(1) > td.colunaApenasBordaEsquerda > label");
                var auxB = element.Text;
                doc.InsertParagraph(auxA + ": " + auxB);
                
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(2) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(2) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                doc.InsertParagraph(auxA + ": " + auxB);

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(3) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(3) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                doc.InsertParagraph(auxA + ": " + auxB);

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(4) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(4) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                doc.InsertParagraph(auxA + ": " + auxB);
                doc.InsertParagraph(" ");
                doc.InsertParagraph(" ");

                element = _driver.FindElementByCssSelector("#box > div > fieldset:nth-child(42) > legend");
                doc.InsertParagraph(element.Text);

                element = _driver.FindElementByCssSelector("#idTablemulta > tbody > tr > td > label");
                doc.InsertParagraph(element.Text);
                doc.InsertParagraph(" ");
                doc.InsertParagraph(" ");

                element = _driver.FindElementByCssSelector("#box > div > fieldset:nth-child(44) > legend");
                doc.InsertParagraph(element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(1) > label");
                doc.InsertParagraph("Retificação: "+element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(2) > label");
                doc.InsertParagraph("Receita: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(3) > label");
                doc.InsertParagraph("Valor Receita: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(4) > label");
                doc.InsertParagraph("Juros/Encargos: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(5) > label");
                doc.InsertParagraph("Multa: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(6) > label");
                doc.InsertParagraph("Valor Total: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(7) > label");
                doc.InsertParagraph("Data: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(8) > label");
                doc.InsertParagraph("Tipo: " + element.Text);                
                
                //CLIQUE NO '+'
                element = _driver.FindElement(By.CssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(9) > input[type=button]"));
                element.Click();                
                
                //PAGAMENTOS
                element = _driver.FindElementByCssSelector("#TABLE_4 > tbody > tr > td:nth-child(1)");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#TABLE_4 > tbody > tr > td:nth-child(2)");
                auxB = element.Text;
                element = _driver.FindElementByCssSelector("#TABLE_4 > tbody > tr > td:nth-child(3)");
                var auxC = element.Text;
                doc.InsertParagraph(auxA + " - " + auxB + " - " + auxC );
                doc.InsertParagraph(" ");

                var returnPgto = VerificaPagamento(element, _driver,"2");

                if (returnPgto)
                {
                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(1) > label");
                    doc.InsertParagraph("Retificação: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(2) > label");
                    doc.InsertParagraph("Receita: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(3) > label");
                    doc.InsertParagraph("Valor Receita: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(4) > label");
                    doc.InsertParagraph("Juros/Encargos: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(5) > label");
                    doc.InsertParagraph("Multa: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(6) > label");
                    doc.InsertParagraph("Valor Total: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(7) > label");
                    doc.InsertParagraph("Data: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(8) > label");
                    doc.InsertParagraph("Tipo: " + element.Text);
                   
                    //CLIQUE NO '+'
                    element = _driver.FindElement(By.CssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(9) > input[type=button]"));
                    element.Click();
                                     
                    //PAGAMENTOS
                    element = _driver.FindElementByCssSelector("#TABLE_5 > tbody > tr > td:nth-child(1)");
                    auxA = element.Text;
                    element = _driver.FindElementByCssSelector("#TABLE_5 > tbody > tr > td:nth-child(2)");
                    auxB = element.Text;
                    element = _driver.FindElementByCssSelector("#TABLE_5 > tbody > tr > td:nth-child(3)");
                    auxC = element.Text;
                    doc.InsertParagraph(auxA + " - " + auxB + " - " + auxC);
                    doc.InsertParagraph(" ");
                }

                doc.Save();

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool VerificaPagamento(IWebElement element, PhantomJSDriver _driver, string id)
        {
            try
            {
                element = _driver.FindElementById("linhaBanco"+id);
                var aux = element.Text;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task AtualizaTelaDebito(Importacao import, string cd_imp)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{cd_imp}", import);
                    resultado.EnsureSuccessStatusCode();

                    LogController.RegistrarLog(_nome_cliente + " - " + "Registro salvo com sucesso.");
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro ao atualizar a DI nº {import.TX_NUM_DEC}.");
            }
        }
    }
}
