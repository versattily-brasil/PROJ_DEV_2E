using AutoMapper;
using Core.Flash2;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Usuario;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Controllers
{
    public class UsuarioController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlUsuario;
        private string _urlModulo;
        private string _urlGrupo;
        private readonly IFlasher _flash;
        #endregion

        public UsuarioController(AppSettings appSettings, IMapper mapper, IFlasher flash)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _flash = flash;
            _urlUsuario = this.appSettings.ApiBaseURL + $"sso/v1/usuario";
            _urlModulo = this.appSettings.ApiBaseURL + $"sso/v1/modulo";
            _urlGrupo = this.appSettings.ApiBaseURL + $"sso/v1/grupo";
        }

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(UsuarioListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlUsuario}" +
                                                                               $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                               $"&pagesize={vm.DataPage.PageSize}" +
                                                                               $"&orderby={vm.DataPage.OrderBy}" +
                                                                               $"&Descending={vm.DataPage.Descending}" +
                                                                               $"&tx_nome={vm.TX_NOME}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Usuario>>();
                        vm.DataPage.UrlSearch = $"usuario?";
                        if (vm.DataPage.Items.Any())
                        {
                            return View("Index", vm);
                        }
                        else
                        {
                            return View("Index", vm).WithInfo("", GenericMessages.ListNull());
                        }
                    }
                }
                return View("Index", vm);
            }
            catch (Exception ex)
            {
                vm.DataPage.Message = ex.Message;
                return View(vm.DataPage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlUsuario}/{id}");
                    result.EnsureSuccessStatusCode();
                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);

                    return View("Form", usuarioViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlUsuario}/{id}");
                    result.EnsureSuccessStatusCode();
                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);

                    return View("Form", usuarioViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(UsuarioViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var usuario = _mapper.Map<Usuario>(itemViewModel);
                if (usuario.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlUsuario}/{usuario.CD_USR}", usuario);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        // _flash.Flash("success", GenericMessages.SucessSave("Usuario"));
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Usuario"));
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var results = await client.GetAsync($"{_urlUsuario}/{0}");
                        results.EnsureSuccessStatusCode();
                        var usuarios = await results.Content.ReadAsAsync<Usuario>();
                        var usuarioViewModels = _mapper.Map<UsuarioViewModel>(usuarios);

                        itemViewModel.Modulo = usuarioViewModels.Modulo;
                        itemViewModel.Grupo = usuarioViewModels.Grupo;
                    }                

                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Usuario", usuario.Messages));
                }
            }
            catch (Exception ex)
            {
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long Id)
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync($"{_urlUsuario}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessSave("Usuario"));
            }
        }
        #endregion

    }
}