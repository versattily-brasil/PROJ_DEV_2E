using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.CEMercante.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www.mercante.transportes.gov.br/g36127/servlet/serpro.siscomex.mercante.servlet.MercanteController";
        public string _urlCeItens = @"https://www.mercante.transportes.gov.br/g36127/servlet/serpro.siscomex.mercante.servlet.MercanteControler?exibeBotaoFechar=1&acao=CE-CON&passo=2&tipoConsulta=&exibeBotao=1&nrCeMercante=";

        private string _urlApiBase;
        private List<TBCEMercante> registros;

        int aux = 0;
        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;
        ChromeOptions options = null;

        #endregion

        public Work()
        {
            LogController.RegistrarLog( "#################  INICIALIZANDO - CE MERCANTE  ################# " );
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work( int cd_bot_exec, int cd_par, string nome_cliente )
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog( "#################  INICIALIZANDO - CE MERCANTE  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot" );
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            using ( var service = PhantomJSDriverService.CreateDefaultService() )
            {
                LogController.RegistrarLog( "Carregando certificado...", eTipoLog.INFO, _cd_bot_exec, "bot" );
                ControleCertificados.CarregarCertificado( service );

                service.AddArgument( "test-type" );
                service.AddArgument( "no-sandbox" );
                //service.HideCommandPromptWindow = true;

                using ( var _driver = new PhantomJSDriver( service ) )
                {
                    try
                    {
                        Acessar( _driver );

                        LogController.RegistrarLog( $"Execução concluída.", eTipoLog.INFO, _cd_bot_exec, "bot" );
                    }
                    catch ( Exception )
                    {
                        _driver.Close();
                    }
                }
            }
        }

        private async Task Acessar( PhantomJSDriver _driver )
        {
            try
            {
                LogController.RegistrarLog( "Acessando URL...", eTipoLog.INFO, _cd_bot_exec, "bot" );

                _driver.Navigate().GoToUrl( _urlSite );
                Thread.Sleep( 500 );
                var retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Clique na Area para Cadastrados", eTipoLog.INFO, _cd_bot_exec, "bot" );
                OpenQA.Selenium.IWebElement element = _driver.FindElementByCssSelector( "body > form:nth-child(3) > table:nth-child(6) > tbody > tr > td:nth-child(4) > p:nth-child(6) > a" );
                element.Click();
                Thread.Sleep( 900 );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Click no Certificado", eTipoLog.INFO, _cd_bot_exec, "bot" );
                element = _driver.FindElementByCssSelector( "#cpfsenha > table.Tab1 > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(4) > td:nth-child(1) > table > tbody > tr:nth-child(4) > td:nth-child(2) > div > a > img" );
                element.Click();
                Thread.Sleep( 500 );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                _driver.SwitchTo().Frame( 1 );
                LogController.RegistrarLog( $"Clique na Aba 'Conhecimento'", eTipoLog.INFO, _cd_bot_exec, "bot" );
                element = _driver.FindElementByCssSelector( "body > form:nth-child(1) > table:nth-child(2) > tbody > tr > td:nth-child(3) > a" );
                element.Click();
                Thread.Sleep( 500 );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Selecionando: Conhecimento/BL/BL-HOUSE", eTipoLog.INFO, _cd_bot_exec, "bot" );
                Select selectTipo = new Select( _driver, By.Name( "cmbAcoes" ) );
                selectTipo.SelectByText( "Conhecimento/BL/BL-HOUSE" );
                Thread.Sleep( 500 );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Selecionando: Consultar Conhecimentos Incluídos e Alterados", eTipoLog.INFO, _cd_bot_exec, "bot" );
                selectTipo = new Select( _driver, By.Name( "cmbAcoesNivel2" ) );
                selectTipo.SelectByText( "Consultar Conhecimentos Incluídos e Alterados" );
                Thread.Sleep( 500 );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                _driver.SwitchTo().DefaultContent();  // .Frame( "main");
                _driver.SwitchTo().Frame( "main" );
                LogController.RegistrarLog( $"Adicionando a Data de Inclusão do Conhecimento", eTipoLog.INFO, _cd_bot_exec, "bot" );
                element = _driver.FindElementById( "DtConsulta" );
                element.SendKeys( "31102019" );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Adicionando o CNPJ/CPF do Consignatário", eTipoLog.INFO, _cd_bot_exec, "bot" );
                element = _driver.FindElementByCssSelector( "#CnpjCpfConsignatario" );
                element.SendKeys( "00280273000137" );
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                LogController.RegistrarLog( $"Clique no Botão enviar", eTipoLog.INFO, _cd_bot_exec, "bot" );
                element = _driver.FindElementById( "Enviar" );
                element.Click();
                retorno = capturaImagem( _driver, aux.ToString() );
                aux++;

                //LEITURA DO CE'S E GRAVACAO NA TABELA PARA FAZER O INCEPTION
                bool leNumCE = true;

                string data = string.Empty;

                int cont = 3;

                TBCEMercante mercante = new TBCEMercante();
                List<TBCEMercante> ListaCE = new List<TBCEMercante>();

                while ( leNumCE )
                {
                    try
                    {
                        mercante.TX_CE_MERCANTE = _driver.FindElement( By.CssSelector( String.Format( "body > form > table:nth-child(10) > tbody > tr:nth-child({0}) > td:nth-child(1) > a", cont ) ) ).Text;
                        mercante.TX_NUM_BL = _driver.FindElement( By.CssSelector( String.Format( "body > form > table:nth-child(10) > tbody > tr:nth-child({0}) > td:nth-child(2)", cont ) ) ).Text;
                        mercante.TX_TIPO = _driver.FindElement( By.CssSelector( String.Format( "body > form > table:nth-child(10) > tbody > tr:nth-child({0}) > td:nth-child(3)", cont ) ) ).Text;
                        mercante.TX_NUM_MANIFESTO = _driver.FindElement( By.CssSelector( String.Format( "body > form > table:nth-child(10) > tbody > tr:nth-child({0}) > td:nth-child(4) > a", cont ) ) ).Text;
                        data = _driver.FindElement( By.CssSelector( String.Format( "body > form > table:nth-child(10) > tbody > tr:nth-child({0}) > td:nth-child(5)", cont ) ) ).Text;
                        mercante.DT_DATA_OPERACAO = Convert.ToDateTime( data );

                        LogController.RegistrarLog( $"Inserindo no Banco o CE Nº " + mercante.TX_CE_MERCANTE, eTipoLog.INFO, _cd_bot_exec, "bot" );
                        await AtualizaCEMercante( mercante );

                        ListaCE.Add( mercante );
                        
                        cont++;
                    }
                    catch
                    {
                        LogController.RegistrarLog( $"Fim da Leitura do CE's", eTipoLog.INFO, _cd_bot_exec, "bot" );
                        leNumCE = false;
                    }
                }

                //Carrega Tabela CE Mercante

                TBCEMercanteItens mercanteItem = new TBCEMercanteItens();
                List<TBCEMercanteItens> ListaCEItem = new List<TBCEMercanteItens>();

                string peso = "";
                bool leNumCEItem = true;
                cont = 3;

                foreach ( var item in ListaCE )
                {
                    LogController.RegistrarLog( "Itens do CE " + item.TX_CE_MERCANTE, eTipoLog.INFO, _cd_bot_exec, "bot" );

                    _driver.Navigate().GoToUrl( _urlCeItens + item.TX_CE_MERCANTE );
                    Thread.Sleep( 500 );
                    retorno = capturaImagem( _driver, aux.ToString() );
                    aux++;

                    //SALVAR TODO O CONTEUDO DA PAGINA ANTES DE INCLUIR O ITEM.....



                    element = _driver.FindElementByCssSelector( "body > fieldset:nth-child(26) > table > tbody > tr:nth-child(3) > td > b > a" );
                    element.Click();
                    Thread.Sleep( 500 );
                    retorno = capturaImagem( _driver, aux.ToString() );
                    aux++;
                   
                    while ( leNumCEItem )
                    {
                        try
                        {
                            mercanteItem.TX_NRO_CE = item.TX_CE_MERCANTE;
                            mercanteItem.TX_CODIGO = _driver.FindElement( By.CssSelector( String.Format( "body > form > fieldset:nth-child(60) > table > tbody > tr:nth-child({0}) > td:nth-child(1) > a", cont ) ) ).Text;
                            mercanteItem.TX_TIPO = _driver.FindElement( By.CssSelector( String.Format( "body > form > fieldset:nth-child(60) > table > tbody > tr:nth-child({0}) > td:nth-child(2)", cont ) ) ).Text;
                            peso = _driver.FindElement( By.CssSelector( String.Format( "body > form > fieldset:nth-child(60) > table > tbody > tr:nth-child(3) > td:nth-child({0})", cont ) ) ).Text;
                            mercanteItem.VL_PESO = Convert.ToDecimal(peso);
                            mercanteItem.TX_PENDENTE = _driver.FindElement( By.CssSelector( String.Format( "body > form > fieldset:nth-child(60) > table > tbody > tr:nth-child({0}) > td:nth-child(4)", cont ) ) ).Text == "NÃO" ? "N":"S" ; 
                            mercanteItem.TX_DETALHAMENTO = _driver.FindElement( By.CssSelector( String.Format( "body > form > fieldset:nth-child(60) > table > tbody > tr:nth-child({0}) > td:nth-child(5)", cont ) ) ).Text;


                            LogController.RegistrarLog( $"Inserindo no Banco o CE Item Nº " + item.TX_CE_MERCANTE + " Codigo: " + mercanteItem.TX_CODIGO, eTipoLog.INFO, _cd_bot_exec, "bot" );
                            await AtualizaCEMercanteItem( mercanteItem );

                            ListaCEItem.Add( mercanteItem );

                            cont++;
                        }
                        catch
                        {
                            LogController.RegistrarLog( $"Fim da Leitura do Item do CE Nº " + item.TX_CE_MERCANTE, eTipoLog.INFO, _cd_bot_exec, "bot" );
                            leNumCEItem = false;
                        }
                    }
                }

            }
            catch ( Exception ex )
            {
                LogController.RegistrarLog( _nome_cliente + " - " + $"Erro em Acessar. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot" );
            }
        }

        private async Task AtualizaCEMercante( TBCEMercante import )
        {
            try
            {
                using ( var client = new HttpClient() )
                {
                    var auxA = JsonConvert.SerializeObject( import );
                    HttpContent httpContent = new StringContent( auxA, Encoding.UTF8, "application/json" );
                    client.BaseAddress = new Uri( _urlApiBase );
                    var resultado = client.PostAsync( $"imp/v1/cemercante", httpContent ).Result;
                    resultado.EnsureSuccessStatusCode();
                }
            }
            catch ( Exception e )
            {
                LogController.RegistrarLog( $"Erro ao atualizar o CE nº {import.TX_CE_MERCANTE}." );
            }
        }

        private async Task AtualizaCEMercanteItem( TBCEMercanteItens import )
        {
            try
            {
                using ( var client = new HttpClient() )
                {
                    var auxA = JsonConvert.SerializeObject( import );
                    HttpContent httpContent = new StringContent( auxA, Encoding.UTF8, "application/json" );
                    client.BaseAddress = new Uri( _urlApiBase );
                    var resultado = client.PostAsync( $"imp/v1/cemercanteitens", httpContent ).Result;
                    resultado.EnsureSuccessStatusCode();
                }
            }
            catch ( Exception e )
            {
                LogController.RegistrarLog( $"Erro ao atualizar o Item do CE nº {import.TX_NRO_CE}." );
            }
        }

        public void Screenshot( IWebDriver driver, string screenshotsPasta )
        {
            ITakesScreenshot camera = driver as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile( screenshotsPasta, ScreenshotImageFormat.Png );
        }

        public bool capturaImagem( PhantomJSDriver _driver, string numero )
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if ( !System.IO.Directory.Exists( @"C:\Versatilly\" ) )
                {
                    System.IO.Directory.CreateDirectory( @"C:\Versatilly\" );
                }

                string arquivoPath = Path.Combine( @"C:\Versatilly\", numero + "-CapturaTelaCE.jpg" );

                Screenshot( _driver, arquivoPath );
                Thread.Sleep( 1000 );

                FileInfo fileInfo = new FileInfo( arquivoPath );
                var tam = fileInfo.Length;
                if ( fileInfo.Length <= 0 )
                {
                    File.Delete( arquivoPath );

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch ( Exception )
            {
                return false;
            }
        }
    }
}
