using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Extensions.Filters;
using P2E.Main.UI.Web.Extensions.Util;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class RotinaController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlRotina, _urlServico;
        #endregion

        #region construtor
        public RotinaController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/rotina";
            _urlServico = this.appSettings.ApiBaseURL + $"sso/v1/servico";
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
        [PermissaoFilter("Rotinas", "Consultar")]
        public async Task<IActionResult> Index(RotinaListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                        var result = await client.GetAsync($"{_urlRotina}" +
                                                                            $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                            $"&pagesize={vm.DataPage.PageSize}" +
                                                                            $"&orderby={vm.DataPage.OrderBy}" +
                                                                            $"&Descending={vm.DataPage.Descending}" +
                                                                            $"&nome={vm.Nome}" +
                                                                            $"&descricao={vm.Descricao}");


                        result.EnsureSuccessStatusCode();

                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Rotina>>();

                        foreach (Rotina item in vm.DataPage.Items.ToList())
                        {
                            var results = await client.GetAsync($"{_urlServico}/{item.CD_SRV}");

                            item.Servico = await results.Content.ReadAsAsync<Servico>();
                        }

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
        [PermissaoFilter("Rotinas", "Editar")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                    var result = await client.GetAsync($"{_urlRotina}/{id}");
                    result.EnsureSuccessStatusCode();
                    var rotina = await result.Content.ReadAsAsync<Rotina>();
                    var rotinaViewModel = _mapper.Map<RotinaViewModel>(rotina);

                    var rotinas = CarregarRotinas().Result;
                    rotinaViewModel.Rotinas = _mapper.Map<List<RotinaViewModel>>(rotinas);

                    // Carregar Rotinas Associadas
                    rotinaViewModel.RotinasAssociadas = CarregarRotinasAssociadas(rotinaViewModel.CD_ROT).Result;

                    return View("Form", rotinaViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [PermissaoFilter("Rotinas", "Visualizar")]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                    var result = await client.GetAsync($"{_urlRotina}/{id}");
                    result.EnsureSuccessStatusCode();
                    var rotina = await result.Content.ReadAsAsync<Rotina>();
                    var rotinaViewModel = _mapper.Map<RotinaViewModel>(rotina);
                    rotinaViewModel.Servicos = CarregarServico().Result;
                    rotinaViewModel.RotinasAssociadas = CarregarRotinasAssociadas(rotinaViewModel.CD_ROT).Result;

                    return View("View", rotinaViewModel);
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
        [PermissaoFilter("Rotinas", "Criar")]
        public IActionResult Create()
        {
            var vm = new RotinaViewModel();
            vm.Servicos = CarregarServico().Result;

            var rotinas = CarregarRotinas().Result;
            vm.Rotinas = _mapper.Map<List<RotinaViewModel>>(rotinas);

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

            var rotina = _mapper.Map<Rotina>(itemViewModel);

            try
            {
                if (rotina.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                        result = await client.PutAsJsonAsync($"{_urlRotina}/{rotina.CD_ROT}", rotina);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Rotina"));
                    }
                }
                else
                {
                    itemViewModel.Servicos = CarregarServico().Result;

                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Rotina", rotina.Messages));
                }
            }
            catch (Exception ex)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                    result = await client.GetAsync($"{_urlRotina}/{0}");
                    result.EnsureSuccessStatusCode();

                    itemViewModel.Servicos = CarregarServico().Result;
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Rotina", responseBody));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [PermissaoFilter("Rotinas", "Deletar")]
        public async Task<IActionResult> Delete(long Id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                    result = await client.DeleteAsync($"{_urlRotina}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();

                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Rotina"));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", new { id = Id }).WithWarning("Erro.", responseBody);
            }
        }

        public IActionResult CancelEdit()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("Rotina"));
        }

        public IActionResult CancelInsert()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.InsertCancel("Rotina"));
        }

        public IActionResult CancelView()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.ShowDetailCancel("Rotina"));
        }

        #endregion        

        private async Task<List<Servico>> CarregarServico()
        {
            string urlServico = this.appSettings.ApiBaseURL + $"sso/v1/servico/todos";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                var result = await client.GetAsync(urlServico);
                var lista = await result.Content.ReadAsAsync<List<Servico>>();
                return lista;
            }
        }

        private async Task<List<Rotina>> CarregarRotinas()
        {
            string urlServico = this.appSettings.ApiBaseURL + $"sso/v1/rotina/todos";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                var result = await client.GetAsync(urlServico);
                var lista = await result.Content.ReadAsAsync<List<Rotina>>();
                return lista;
            }
        }

        private async Task<List<RotinaAssociada>> CarregarRotinasAssociadas(int id)
        {
            string url = this.appSettings.ApiBaseURL + $"sso/v1/rotina/associadas/{id}";
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirst("api_token").Value);
                var result = await client.GetAsync(url);
                var lista = await result.Content.ReadAsAsync<List<RotinaAssociada>>();
                return lista;
            }
        }

    }
}