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
    public class UsuarioModuloController : Controller
    {
        private readonly AppSettings appSettings;

        public UsuarioModuloController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/usuarioModulo");
            result.EnsureSuccessStatusCode();
            List<UsuarioModuloVM> list = await result.Content.ReadAsAsync<List<UsuarioModuloVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/usuarioModulo/" + id);
                result.EnsureSuccessStatusCode();

                UsuarioModuloVM modulo = await result.Content.ReadAsAsync<UsuarioModuloVM>();

                return View(modulo);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioModuloVM modulo)
        {
            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<UsuarioModuloVM>(this.appSettings.ApiBaseURL + "/usuarioModulo/" + modulo.CD_USR_MOD, modulo);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi salvo corretamente."); ;
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(this.appSettings.ApiBaseURL + "/usuarioModulo/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi excluído corretamente.");

        }
    }
}