using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.API.ViewModel;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Extensions.Util;
using P2E.Main.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class TabelaAuxiliarController : Controller
    {
        private readonly AppSettings appSettings;

        public TabelaAuxiliarController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/tabelaauxiliar");
            result.EnsureSuccessStatusCode();
            List<TabelaAuxiliarVM> list = await result.Content.ReadAsAsync<List<TabelaAuxiliarVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(string id)
        {
            if (id != string.Empty)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/tabelaauxiliar/" + id);
                result.EnsureSuccessStatusCode();

                TabelaAuxiliarVM exemplo = await result.Content.ReadAsAsync<TabelaAuxiliarVM>();

                return View(exemplo);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(TabelaAuxiliarVM tabela)
        {
            if (tabela.TX_TABELA == String.Empty)
            {
                return View(tabela).WithDanger("Erro.", "Preencha todos os campos.");
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
            await client.PutAsJsonAsync<TabelaAuxiliarVM>(this.appSettings.ApiBaseURL + "/tabelaauxiliar/" + tabela.TX_TABELA, tabela);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Exemplo foi salvo corretamente.");
        }

        public async Task<IActionResult> Excluir(string Id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
            await client.DeleteAsync(this.appSettings.ApiBaseURL + "/tabelaauxiliar/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "A Tabela foi excluída corretamente.");

        }
    }
}