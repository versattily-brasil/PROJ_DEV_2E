using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    public class RotinaController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlRotina;
        #endregion

        #region construtor
        public RotinaController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/rotina";
        }
        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="decricao"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(RotinaListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlRotina}" +
                                                                            $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                            $"&pagesize={vm.DataPage.PageSize}" +
                                                                            $"&orderby={vm.DataPage.OrderBy}" +
                                                                            $"&Descending={vm.DataPage.Descending}" +
                                                                            $"&nome={vm.Nome}" +
                                                                            $"&descricao={vm.Descricao}");


                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Rotina>>();
                        vm.DataPage.UrlSearch = $"rotina?";
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
                    var result = await client.GetAsync($"{_urlRotina}/{id}");
                    result.EnsureSuccessStatusCode();
                    var rotina = await result.Content.ReadAsAsync<Rotina>();
                    var rotinaViewModel = _mapper.Map<RotinaViewModel>(rotina);
                    return View("Form", rotinaViewModel);
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
        public async Task<IActionResult> Save(RotinaViewModel itemViewModel)
        {
            try
            {
                var rotina = _mapper.Map<Rotina>(itemViewModel);
                if (rotina.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.PutAsJsonAsync($"{_urlRotina}/{rotina.CD_ROT}", rotina);
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Rotina"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Rotina", rotina.Messages));
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
                await client.DeleteAsync($"{_urlRotina}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Rotina"));
            }
        }
        #endregion        
    }
}