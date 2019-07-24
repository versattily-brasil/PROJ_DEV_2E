using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2E.SSO.API.ViewModel;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;

namespace P2E.Main.UI.Web.Controllers
{
    public class ModuloController : Controller
    {
        private readonly AppSettings appSettings;

        public ModuloController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/modulo");
            result.EnsureSuccessStatusCode();
            List<ModuloVM> list = await result.Content.ReadAsAsync<List<ModuloVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/modulo/" + id);
                result.EnsureSuccessStatusCode();

                ModuloVM modulo = await result.Content.ReadAsAsync<ModuloVM>();

                return View(modulo);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(ModuloVM modulo)
        {
            if (modulo.TX_DSC == String.Empty)
            {
                return View(modulo).WithDanger("Erro.", "Preencha todos os campos.");
            }

            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<ModuloVM>(this.appSettings.ApiBaseURL + "/modulo/" + modulo.CD_MOD, modulo);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi salvo corretamente."); ;
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(this.appSettings.ApiBaseURL + "/modulo/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi excluído corretamente.");

        }
    }
}