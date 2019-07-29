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
    public class UsuarioController : Controller
    {
        private readonly AppSettings appSettings;
        private string _urlUsuario;

        public UsuarioController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            _urlUsuario = this.appSettings.ApiBaseURL + $"sso/v1/usuario";
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(_urlUsuario);
            result.EnsureSuccessStatusCode();
            List<UsuarioVM> list = await result.Content.ReadAsAsync<List<UsuarioVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(_urlUsuario +"/" + id);
                result.EnsureSuccessStatusCode();

                UsuarioVM usuario = await result.Content.ReadAsAsync<UsuarioVM>();

                return View(usuario);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioVM usuario)
        {
            //if (exemplo.Descricao == String.Empty || exemplo.Valor <= 0)
            //{
            //    return View(exemplo).WithDanger("Erro.", "Preencha todos os campos.");
            //}

            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<UsuarioVM>(_urlUsuario + "/" + usuario.CD_USR, usuario);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Usuário foi salvo corretamente.");
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(_urlUsuario + "/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Usuário foi excluído corretamente.");

        }
    }
}