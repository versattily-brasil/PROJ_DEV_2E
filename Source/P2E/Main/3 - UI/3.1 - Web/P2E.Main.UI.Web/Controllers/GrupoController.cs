using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Core.Flash2;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Grupo;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
namespace P2E.Main.UI.Web.Controllers
{
    public class GrupoController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlGrupo;
        private readonly IFlasher _flash;
        #endregion

        #region construtor
        public GrupoController(AppSettings appSettings, IMapper mapper, IFlasher flash)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _flash = flash;
            _urlGrupo = this.appSettings.ApiBaseURL + $"sso/v1/grupo";
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
        public async Task<IActionResult> Index(GrupoListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlGrupo}" +
                                                                               $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                               $"&pagesize={vm.DataPage.PageSize}" +
                                                                               $"&orderby={vm.DataPage.OrderBy}" +
                                                                               $"&Descending={vm.DataPage.Descending}" +
                                                                               $"&tx_dsc={vm.TX_DSC}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Grupo>>();
                        vm.DataPage.UrlSearch = $"grupo?";
                        return View("Index", vm);
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
                    var result = await client.GetAsync($"{_urlGrupo}/{id}");
                    result.EnsureSuccessStatusCode();
                    var grupo = await result.Content.ReadAsAsync<Grupo>();
                    var grupoViewModel = _mapper.Map<GrupoViewModel>(grupo);
                    return View("Form", grupoViewModel);
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
        public async Task<IActionResult> Save(GrupoViewModel itemViewModel)
        {
            try
            {
                var grupo = _mapper.Map<Grupo>(itemViewModel);
                if (grupo.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        await client.PutAsJsonAsync($"{_urlGrupo}/{grupo.CD_GRP}", grupo);
                        _flash.Flash("success", GenericMessages.SucessSave("Grupo"));
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Grupo"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Grupo", grupo.Messages));
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
                await client.DeleteAsync($"{_urlGrupo}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessSave("Grupo"));
            }
        }
        #endregion
    }
}