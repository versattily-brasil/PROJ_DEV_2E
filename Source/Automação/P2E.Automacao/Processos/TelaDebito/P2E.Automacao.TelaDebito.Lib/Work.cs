using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.TelaDebito.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlTelaConsulta = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/ConsultarDIMenu.do";

        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - TELA DE DÉBITO  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
            //_urlApiBase = "http://localhost:7000/";
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para Consultar Tela de Débito.");
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
                registros = JsonConvert.DeserializeObject<List<Importacao>>(aux);

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
                    Console.WriteLine("Não existe DI's para Acompanhar Despacho.");
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver, Importacao import, string nroDI)
        {
            var numDeclaracao = numero;

            Console.WriteLine("ACESSAO PAGINA DE CONSULTA...");
            _driver.Navigate().GoToUrl(_urlTelaConsulta);

            //obtendo o campo de numero de declaração.
            IWebElement element = _driver.FindElementById("nrDeclaracao");

            Console.WriteLine("inserindo o numero da declaração");
            element.SendKeys(numero);
          
            element = _driver.FindElement(By.Name("enviar"));
            element.Click();
            Thread.Sleep(2000);

            Console.WriteLine("Criando Arquivo.....");

            var retornFile = CreateDocumentWord(element, _driver, numero);

            Console.WriteLine("Gravando Status...");
            
            import.OP_TELA_DEBITO = retornFile ? 1 : 0;

            await AtualizaTelaDebito(import, nroDI);

            Console.WriteLine("Arquivo Criado...");
        }

        public bool CreateDocumentWord(IWebElement element, PhantomJSDriver _driver,string numero)
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                }

                string arquivoPath = Path.Combine("C:\\Versatilly\\", numero + "-TelaDebito.doc");

                StreamWriter writer = new StreamWriter(arquivoPath);

                element = _driver.FindElementByCssSelector("#TABLE_1 > tbody > tr > td.tituloDI");
                writer.WriteLine(element.Text);
                writer.WriteLine(" ");
                writer.WriteLine(" ");

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(1) > td.colunaSemBorda > label");
                var auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(1) > td.colunaApenasBordaEsquerda > label");
                var auxB = element.Text;
                writer.WriteLine(auxA + ": " + auxB);
                
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(2) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(2) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                writer.WriteLine(auxA + ": " + auxB);

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(3) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(3) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                writer.WriteLine(auxA + ": " + auxB);

                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(4) > td:nth-child(1) > label");
                auxA = element.Text;
                element = _driver.FindElementByCssSelector("#idTableDocInstDes > tbody > tr:nth-child(4) > td.colunaBordaEsquerda > label");
                auxB = element.Text;
                writer.WriteLine(auxA + ": " + auxB);
                writer.WriteLine(" ");
                writer.WriteLine(" ");

                element = _driver.FindElementByCssSelector("#box > div > fieldset:nth-child(42) > legend");
                writer.WriteLine(element.Text);

                element = _driver.FindElementByCssSelector("#idTablemulta > tbody > tr > td > label");
                writer.WriteLine(element.Text);
                writer.WriteLine(" ");
                writer.WriteLine(" ");

                element = _driver.FindElementByCssSelector("#box > div > fieldset:nth-child(44) > legend");
                writer.WriteLine(element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(1) > label");
                writer.WriteLine("Retificação: "+element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(2) > label");
                writer.WriteLine("Receita: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(3) > label");
                writer.WriteLine("Valor Receita: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(4) > label");
                writer.WriteLine("Juros/Encargos: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(5) > label");
                writer.WriteLine("Multa: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(6) > label");
                writer.WriteLine("Valor Total: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(7) > label");
                writer.WriteLine("Data: " + element.Text);

                element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(2) > td:nth-child(8) > label");
                writer.WriteLine("Tipo: " + element.Text);                
                
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
                writer.WriteLine(auxA + " - " + auxB + " - " + auxC );
                writer.WriteLine(" ");

                var returnPgto = VerificaPagamento(element, _driver,"2");

                if (returnPgto)
                {
                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(1) > label");
                    writer.WriteLine("Retificação: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(2) > label");
                    writer.WriteLine("Receita: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(3) > label");
                    writer.WriteLine("Valor Receita: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(4) > label");
                    writer.WriteLine("Juros/Encargos: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(5) > label");
                    writer.WriteLine("Multa: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(6) > label");
                    writer.WriteLine("Valor Total: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(7) > label");
                    writer.WriteLine("Data: " + element.Text);

                    element = _driver.FindElementByCssSelector("#idTablePagamentos > tbody > tr:nth-child(4) > td:nth-child(8) > label");
                    writer.WriteLine("Tipo: " + element.Text);
                   
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
                    writer.WriteLine(auxA + " - " + auxB + " - " + auxC);
                    writer.WriteLine(" ");
                }

                writer.Close();

                return true;
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
