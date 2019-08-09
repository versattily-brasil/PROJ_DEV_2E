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
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    public class ModuloController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlModulo;
        #endregion

        #region construtor
        public ModuloController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlModulo = this.appSettings.ApiBaseURL + $"sso/v1/modulo";
        }
        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(ModuloListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result =  await client.GetAsync($"{_urlModulo}" +
                                                                               $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                               $"&pagesize={vm.DataPage.PageSize}" +
                                                                               $"&orderby={vm.DataPage.OrderBy}" +
                                                                               $"&Descending={vm.DataPage.Descending}" +
                                                                               $"&tx_dsc={vm.TX_DSC}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Modulo>>();
                        vm.DataPage.UrlSearch = $"modulo?";
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
                    var result = await client.GetAsync($"{_urlModulo}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<Modulo>();
                    var moduloViewModel = _mapper.Map<ModuloViewModel>(modulo);
                    return View("Form", moduloViewModel);
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
        public IActionResult Create()
        {
            return View("Form");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(ModuloViewModel itemViewModel)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(itemViewModel);
                if (modulo.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        await client.PutAsJsonAsync($"{_urlModulo}/{modulo.CD_MOD}", modulo);
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Módulo"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Modulo", modulo.Messages));
                }
            }
            catch (Exception ex)
            {
                return View("Form", itemViewModel).WithDanger("Erro", ex.Message);
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
                await client.DeleteAsync($"{_urlModulo}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Módulo"));
            }
        }
        #endregion

        //private readonly AppSettings appSettings;

        //public ModuloController(AppSettings appSettings)
        //{
        //    this.appSettings = appSettings;
        //}

        //[HttpGet]
        //public async Task<IActionResult> Lista()
        //{
        //    HttpClient client = new HttpClient();
        //    var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/modulo");
        //    result.EnsureSuccessStatusCode();
        //    List<ModuloVM> list = await result.Content.ReadAsAsync<List<ModuloVM>>();

        //    return View(list);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Cadastro(int id)
        //{
        //    if (id != 0)
        //    {
        //        HttpClient client = new HttpClient();
        //        var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/modulo/" + id);
        //        result.EnsureSuccessStatusCode();

        //        ModuloVM modulo = await result.Content.ReadAsAsync<ModuloVM>();

        //        return View(modulo);
        //    }

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Cadastro(ModuloVM modulo)
        //{
        //    if (modulo.TX_DSC == String.Empty)
        //    {
        //        return View(modulo).WithDanger("Erro.", "Preencha todos os campos.");
        //    }

        //    HttpClient client = new HttpClient();
        //    await client.PutAsJsonAsync<ModuloVM>(this.appSettings.ApiUsuarioBaseURL + "/modulo/" + modulo.CD_MOD, modulo);
        //    return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi salvo corretamente."); ;
        //}

        //public async Task<IActionResult> Excluir(int Id)
        //{
        //    HttpClient client = new HttpClient();
        //    await client.DeleteAsync(this.appSettings.ApiUsuarioBaseURL + "/modulo/" + Id);
        //    return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Módulo foi excluído corretamente.");

        //}
    }
}