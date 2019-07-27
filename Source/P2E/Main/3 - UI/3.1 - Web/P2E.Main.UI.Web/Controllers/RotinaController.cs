using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.SSO.API.ViewModel;

namespace P2E.Main.UI.Web.Controllers
{
    public class RotinaController : Controller
    {
        private readonly AppSettings appSettings;

        public RotinaController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/rotina");
            result.EnsureSuccessStatusCode();
            List<RotinaVM> list = await result.Content.ReadAsAsync<List<RotinaVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/rotina/" + id);
                result.EnsureSuccessStatusCode();

                RotinaVM rotina = await result.Content.ReadAsAsync<RotinaVM>();

                return View(rotina);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(RotinaVM rotina)
        {
            //if (exemplo.Descricao == String.Empty || exemplo.Valor <= 0)
            //{
            //    return View(exemplo).WithDanger("Erro.", "Preencha todos os campos.");
            //}

            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<RotinaVM>(this.appSettings.ApiBaseURL + "/rotina/" + rotina.CD_ROT, rotina);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "A Rotina foi salva corretamente.");
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(this.appSettings.ApiBaseURL + "/rotina/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "A Rotina foi excluída corretamente.");

        }
    }
}