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
using P2E.Main.UI.Web.Models.SSO.ParceiroNegocio;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    public class ParceiroNegocioController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlParceiro;
        private readonly IFlasher _flash;
        #endregion

        #region construtor
        public ParceiroNegocioController(AppSettings appSettings, IMapper mapper, IFlasher flash)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _flash = flash;
            _urlParceiro = this.appSettings.ApiBaseURL + $"sso/v1/parceironegocio";
        }
        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="razaosocial"></param>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(ParceiroNegocioListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {

                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlParceiro}?currentpage={vm.DataPage.CurrentPage}&pagesize={vm.DataPage.PageSize}&orderby={vm.DataPage.OrderBy}&Descending={vm.DataPage.Descending}&cnpj={vm.cnpj}&razaosocial={vm.razaosocial}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<ParceiroNegocio>>();
                        vm.DataPage.UrlSearch = $"parceironegocio?";
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
                    var result = await client.GetAsync($"{_urlParceiro}/{id}");
                    result.EnsureSuccessStatusCode();
                    var parceiroNegocio = await result.Content.ReadAsAsync<ParceiroNegocio>();
                    var parceiroNegocioViewModel = _mapper.Map<ParceiroNegocioViewModel>(parceiroNegocio);
                    return View("Form", parceiroNegocioViewModel);
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
        public async Task<IActionResult> Save(ParceiroNegocioViewModel itemViewModel)
        {
            try
            {
                var parceiroNegocio = _mapper.Map<ParceiroNegocio>(itemViewModel);
                if (parceiroNegocio.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        await client.PutAsJsonAsync($"{_urlParceiro}/{parceiroNegocio.CD_PAR}", parceiroNegocio);
                        _flash.Flash("success", GenericMessages.SucessSave("Parceiro Negócio"));
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Parceiro Negócio"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Parceiro Negocio", parceiroNegocio.Messages));
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
                await client.DeleteAsync($"{_urlParceiro}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessSave("Parceiro Negócio"));
            }
        } 
        #endregion
    }
}