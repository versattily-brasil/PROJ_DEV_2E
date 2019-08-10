using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class OperacaoController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlOperacao;
        #endregion

        #region construtor
        public OperacaoController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlOperacao = this.appSettings.ApiBaseURL + $"sso/v1/operacao";
        }
        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="decricao"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(OperacaoListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlOperacao}" +
                                                                            $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                            $"&pagesize={vm.DataPage.PageSize}" +
                                                                            $"&orderby={vm.DataPage.OrderBy}" +
                                                                            $"&Descending={vm.DataPage.Descending}" +
                                                                            $"&tx_dsc={vm.TX_DSC}");


                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Operacao>>();
                        vm.DataPage.UrlSearch = $"operacao?";
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
                    var result = await client.GetAsync($"{_urlOperacao}/{id}");
                    result.EnsureSuccessStatusCode();
                    var operacao = await result.Content.ReadAsAsync<Operacao>();
                    var operacaoViewModel = _mapper.Map<OperacaoViewModel>(operacao);
                    return View("Form", operacaoViewModel);
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
        public async Task<IActionResult> Save(OperacaoViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var operacao = _mapper.Map<Operacao>(itemViewModel);
                if (operacao.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlOperacao}/{operacao.CD_OPR}", operacao);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Operacao"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Operacao", operacao.Messages));
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
                await client.DeleteAsync($"{_urlOperacao}/{Id}");
                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Operacao"));
            }
        }
        #endregion        
    }
}