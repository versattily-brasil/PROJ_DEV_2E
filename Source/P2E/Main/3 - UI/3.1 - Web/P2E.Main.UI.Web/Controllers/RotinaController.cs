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
    [Authorize]
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
                    rotinaViewModel.Grupos = CarregarGrupos().Result;
                    rotinaViewModel.Operacoes = CarregarOperacoes().Result;

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
            var vm = new RotinaViewModel();
            vm.Grupos = CarregarGrupos().Result;
            vm.Operacoes = CarregarOperacoes().Result;
            return View("Form", vm);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(RotinaViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var rotina = _mapper.Map<Rotina>(itemViewModel);
                if (rotina.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlRotina}/{rotina.CD_ROT}", rotina);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Rotina"));
                    }
                }
                else
                {
                    itemViewModel.Grupos = CarregarGrupos().Result;
                    itemViewModel.Operacoes = CarregarOperacoes().Result;

                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Rotina", rotina.Messages));
                }
            }
            catch (Exception ex)
            {
                itemViewModel.Grupos = CarregarGrupos().Result;
                itemViewModel.Operacoes = CarregarOperacoes().Result;
                return View("Form", itemViewModel).WithDanger("Erro", result.RequestMessage.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long Id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                result = await client.DeleteAsync($"{_urlRotina}/{Id}");
                result.EnsureSuccessStatusCode();

                return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessSave("Rotina"));
            }
        }
        #endregion        

        private async Task<List<Grupo>> CarregarGrupos()
        {
            string urlGrupo = this.appSettings.ApiBaseURL + $"sso/v1/grupo/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlGrupo);
                var lista = await result.Content.ReadAsAsync<List<Grupo>>();
                return lista;
            }
        }

        private async Task<List<Operacao>> CarregarOperacoes()
        {
            string urlOperacao = this.appSettings.ApiBaseURL + $"sso/v1/operacao/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlOperacao);
                var lista = await result.Content.ReadAsAsync<List<Operacao>>();
                return lista;
            }
        }
    }
}