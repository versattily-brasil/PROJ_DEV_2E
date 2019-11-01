using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Administrativo.Domain.Entities;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.ADM.ProgramacaoBot;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Controllers
{
    //[Route("programacaobot")]
    public class ProgramacaoBotController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlAgenda, _urlServico;
        #endregion

        #region construtor
        public ProgramacaoBotController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlAgenda = this.appSettings.ApiBaseURL + $"adm/v1/agenda";
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
        //[PermissaoFilter("programacaobot", "Consultar")]
        public async Task<IActionResult> Index(AgendaListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        string url = $"{_urlAgenda}" +
                                                                            $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                            $"&pagesize={vm.DataPage.PageSize}" +
                                                                            $"&orderby={vm.DataPage.OrderBy}" +
                                                                            $"&Descending={vm.DataPage.Descending}" +
                                                                            $"&descricao={vm.Descricao}";
                        var result = await client.GetAsync(url);


                        result.EnsureSuccessStatusCode();

                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Agenda>>();

                        //foreach (Agenda item in vm.DataPage.Items.ToList())
                        //{ 
                        //    var results= await client.GetAsync($"{_urlServico}/{item.CD_SRV}");

                        //    item.Servico = await results.Content.ReadAsAsync<Agenda>();
                        //}

                        vm.DataPage.UrlSearch = $"programacao-bot?";
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
        //[PermissaoFilter("programacao-bot", "Editar")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlAgenda}/{id}");
                    result.EnsureSuccessStatusCode();
                    var agenda = await result.Content.ReadAsAsync<Agenda>();
                    var programacaoBotViewModel = _mapper.Map<AgendaViewModel>(agenda);
                    //rotinaViewModel.Servicos = CarregarServico().Result;                    

                    return View("Form", programacaoBotViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        //[PermissaoFilter("programacao-bot", "Visualizar")]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlAgenda}/{id}");
                    result.EnsureSuccessStatusCode();
                    var rotina = await result.Content.ReadAsAsync<Rotina>();
                    var rotinaViewModel = _mapper.Map<RotinaViewModel>(rotina);
                    rotinaViewModel.Servicos = CarregarServico().Result;

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
        //[PermissaoFilter("programacao-bot", "Criar")]
        public IActionResult Create()
        {
            var vm = new RotinaViewModel();
            vm.Servicos = CarregarServico().Result;
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
                        result = await client.PutAsJsonAsync($"{_urlAgenda}/{rotina.CD_ROT}", rotina);
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
                    result = await client.GetAsync($"{_urlAgenda}/{0}");
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
        //[PermissaoFilter("programacao-bot", "Deletar")]
        public async Task<IActionResult> Delete(long Id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlAgenda}/{Id}");
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
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("programacao-bot"));
        }

        public IActionResult CancelInsert()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.InsertCancel("programacao-bot"));
        }

        public IActionResult CancelView()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.ShowDetailCancel("programacao-bot"));
        }

        #endregion        

        private async Task<List<Servico>> CarregarServico()
        {
            string urlServico = this.appSettings.ApiBaseURL + $"sso/v1/servico/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlServico);
                var lista = await result.Content.ReadAsAsync<List<Servico>>();
                return lista;
            }
        }


    }
}