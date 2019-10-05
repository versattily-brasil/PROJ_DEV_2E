using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Extensions.Filters;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.IMP.DetalheNCM;
using P2E.Shared.Message;
using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class DetalheNCMController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlDetalheNCM;
        #endregion

        #region construtor
        public DetalheNCMController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlDetalheNCM = this.appSettings.ApiBaseURL + $"imp/v1/detalhencm";
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
        [PermissaoFilter("Detalhes NCM", "Consultar")]
        public async Task<IActionResult> Index(DetalheNCMListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlDetalheNCM}" +
                                                           $"?currentpage={vm.DataPage.CurrentPage}" +
                                                           $"&pagesize={vm.DataPage.PageSize}" +
                                                           $"&orderby={vm.DataPage.OrderBy}" +
                                                           $"&Descending={vm.DataPage.Descending}" +
                                                           $"&TX_SFNCM_DESCRICAO={vm.descricao}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<DetalheNCM>>();
                        vm.DataPage.UrlSearch = $"detalhencm?";
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
        [PermissaoFilter("Detalhes NCM", "Editar")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlDetalheNCM}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<DetalheNCM>();
                    var moduloViewModel = _mapper.Map<DetalheNCMViewModel>(modulo);
                    return View("Form", moduloViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [PermissaoFilter("Detalhes NCM", "Visualizar")]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlDetalheNCM}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<DetalheNCM>();
                    var moduloViewModel = _mapper.Map<DetalheNCMViewModel>(modulo);
                    return View("View", moduloViewModel);
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
        [PermissaoFilter("Detalhes NCM", "Criar")]
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
        public async Task<IActionResult> Save(DetalheNCMViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var modulo = _mapper.Map<DetalheNCM>(itemViewModel);
                if (modulo.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlDetalheNCM}/{modulo.CD_DET_NCM}", modulo);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("DetalheNCM"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("DetalheNCM", modulo.Messages));
                }
            }
            catch (Exception ex)
            {
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        public IActionResult CancelView()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.ShowDetailCancel("DetalheNCM"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [PermissaoFilter("Detalhes NCM", "Deletar")]
        public async Task<IActionResult> Delete(long Id)
        {
            string responseBody = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage();

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlDetalheNCM}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();
                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("DetalheNCM"));
                }

            }
            catch (Exception)
            {
                return RedirectToAction("View", new { id = Id }).WithWarning("Erro.", responseBody);
            }
        }
        #endregion
    }
}