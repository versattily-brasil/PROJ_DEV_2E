using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using P2E.Automacao.ExonerarIcms.Lib.DTO;
using P2E.Automacao.Shared.Extensions;
using Selenium.Utils.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P2E.Automacao.ExonerarIcms.Lib
{

    public class Work
    {
        private string uUrlInicio = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/inicio.html";
        public string uUrlbASE = @"https://www1c.siscomex.receita.fazenda.gov.br/";
        public string urlInicioPrivado = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string urlDeclararICMS = @"https://www1c.siscomex.receita.fazenda.gov.br/importacaoweb-7/DeclararICMSMenu.do?i=0";
        private string _urlApiBase;

        private List<ImportacaoDTO> registros;

        public Work()
        {
            Console.WriteLine("ROBÔ 04 – EXONERAÇÃO DO ICMS");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            Console.WriteLine("Obtendo DI's para exoneração.");
            await CarregarListaDIAsync();

            if (registros != null && registros.Any())
            {
                using (var service = PhantomJSDriverService.CreateDefaultService())
                {
                    ControleCertificados.CarregarCertificado(service);

                    service.AddArgument("test-type");
                    service.AddArgument("no-sandbox");
                    service.HideCommandPromptWindow = true;

                    using (var _driver = new PhantomJSDriver(service))
                    {
                        foreach (var di in registros)
                        {
                            ExonerarDIAsync(_driver, di);
                        }
                    }
                }
            }
            else {
                Console.WriteLine("Não existe DI's pendente de exoneração.");
            }
        }

        private async Task ExonerarDIAsync(PhantomJSDriver _driver, ImportacaoDTO di)
        {
            Console.WriteLine($"Exonerando DI nº {di.TX_NUM_DEC}.");
            _driver.Navigate().GoToUrl(urlInicioPrivado);

            _driver.Navigate().GoToUrl(urlDeclararICMS);

            Select selectObject = new Select(_driver, By.Id("tp"));

            selectObject.SelectByText("Exoneração do ICMS");

            IWebElement element = _driver.FindElement(By.Id("numDI"));

            element.SendKeys(di.TX_NUM_DEC);

            Select selectUf = new Select(_driver, By.Id("uf"));

            selectUf.SelectByText(di.UF_DI);

            element = _driver.FindElement(By.Id("registrar"));

            element.Click();

            string txtInicioBlocoErro = "<!-- MENSAGENS DE ERRO =================================================================================================================================== -->";

            int posicaoInicioErro = _driver.PageSource.ToString().IndexOf(txtInicioBlocoErro);

            if (posicaoInicioErro > 0 && _driver.PageSource.Contains("alert"))
            {
                posicaoInicioErro = posicaoInicioErro + txtInicioBlocoErro.Length;

                var posicaoFimErro = _driver.PageSource.ToString().LastIndexOf(");");

                string textoAreaErro = _driver.PageSource.Substring(posicaoInicioErro, posicaoFimErro - posicaoInicioErro);
                textoAreaErro = textoAreaErro.Replace("\"", "'");
                textoAreaErro = textoAreaErro.Replace("<script charset='utf-8' type='text/javascript'>alert('", "");
                textoAreaErro = textoAreaErro.Replace("'+ '\\n' +''", "");
                textoAreaErro = Regex.Replace(textoAreaErro, @"[^ 0-9a-zA-Z]+", "");
                textoAreaErro = Regex.Replace(textoAreaErro, "(\r\n|\r|\n|\t|\r\n\t)", "");

                 Console.WriteLine(textoAreaErro);
            }
            else
            {
                Console.WriteLine("Operação finalizada sem erros.");
                await AtualizarRegistroAsync(di);
            }
        }

        private async Task CarregarListaDIAsync()
        {
            string urlExoneracao = _urlApiBase + $"imp/v1/importacao/obter-exoneracao-icms";

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlExoneracao);
                registros = await result.Content.ReadAsAsync<List<ImportacaoDTO>>();
            }
        }

        private async Task AtualizarRegistroAsync(ImportacaoDTO item)
        {
            Console.WriteLine($"Atualizando DI nº {item.TX_NUM_DEC}.");

            try
            {
                item.DT_DATA_EXO_ICMS = DateTime.Now;

                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);

                    resultado = await client.PutAsJsonAsync($"imp/v1/importacao/{item.CD_IMP}", item);

                    resultado.EnsureSuccessStatusCode();

                    Console.WriteLine("Registro salvo com sucesso.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Erro ao atualizar a DI nº {item.TX_NUM_DEC}.");
            }
        }

    }
}
