using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.Shared.Extensions;
using P2E.Automacao.Shared.Log;
using P2E.Automacao.Shared.Log.Enum;
using System;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.CEMercante.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";

        private string _urlApiBase;

        int _cd_bot_exec;
        int _cd_par;
        string _nome_cliente;

        #endregion

        public Work()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - CE MERCANTE  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public Work(int cd_bot_exec, int cd_par, string nome_cliente)
        {
            _cd_bot_exec = cd_bot_exec;
            _cd_par = cd_par;
            _nome_cliente = nome_cliente;

            LogController.RegistrarLog(_nome_cliente + " - " + "#################  INICIALIZANDO - CE MERCANTE  ################# ", eTipoLog.INFO, _cd_bot_exec, "bot");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            LogController.RegistrarLog(_nome_cliente + " - " + "Obtendo DI's ", eTipoLog.INFO, _cd_bot_exec, "bot");
            await CarregarListaDIAsync();
        }

        private async Task CarregarListaDIAsync()
        {
            using (var service = PhantomJSDriverService.CreateDefaultService())
            {
                LogController.RegistrarLog(_nome_cliente + " - " + "Carregando certificado...", eTipoLog.INFO, _cd_bot_exec, "bot");
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
                {
                    _driver.Navigate().GoToUrl(_urlSite);

                    try
                    {


                        LogController.RegistrarLog(_nome_cliente + " - " + $"Execução concluída.", eTipoLog.INFO, _cd_bot_exec, "bot");

                    }
                    catch (Exception)
                    {
                        _driver.Close();
                    }
                }
            }
        }

        private async Task Acessar(string numero, PhantomJSDriver _driver)
        {
            try
            {
                var numDeclaracao = numero;

                LogController.RegistrarLog(_nome_cliente + " - " + "Acessando URL...", eTipoLog.INFO, _cd_bot_exec, "bot");

                _driver.Navigate().GoToUrl(_urlSite);


            }
            catch (Exception ex)
            {
                LogController.RegistrarLog(_nome_cliente + " - " + $"Erro em Acessar. {ex.Message}", eTipoLog.ERRO, _cd_bot_exec, "bot");
            }
        }
    }
}
