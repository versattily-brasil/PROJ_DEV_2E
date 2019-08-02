using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Main.UI.Web.Models.SSO.Usuario;
using P2E.Shared.Model;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;

        public UsuarioController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListaModulo()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/modulo");
            result.EnsureSuccessStatusCode();
            List<ModuloVM> list = await result.Content.ReadAsAsync<List<ModuloVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/usuario");
            result.EnsureSuccessStatusCode();
            List<UsuarioVM> list = await result.Content.ReadAsAsync<List<UsuarioVM>>();
            
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            HttpClient client = new HttpClient();
            var modResult = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/modulo/");
            modResult.EnsureSuccessStatusCode();
            var pageModulos = await modResult.Content.ReadAsAsync<DataPage<Modulo>>();

            var usuarioVM = new UsuarioViewModel();
            usuarioVM.Modulos = _mapper.Map<List<ModuloViewModel>>(pageModulos.Items);

            if (id != 0)
            {
                var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/usuario/" + id);
                result.EnsureSuccessStatusCode();

                Usuario usuario = await result.Content.ReadAsAsync<Usuario>();

                usuarioVM = _mapper.Map<UsuarioViewModel>(usuario);
                usuarioVM.Modulos = _mapper.Map<List<ModuloViewModel>>(pageModulos.Items);
                return View(usuarioVM);
            }
            
            return View(usuarioVM);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioViewModel usuarioVM)
        {
            var usuario = _mapper.Map<Usuario>(usuarioVM);

            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<Usuario>(this.appSettings.ApiUsuarioBaseURL + "/usuario/" + usuario.CD_USR, usuario);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Usuário foi salvo corretamente.");
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(this.appSettings.ApiUsuarioBaseURL + "/usuario/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Usuário foi excluído corretamente.");

        }
    }
}