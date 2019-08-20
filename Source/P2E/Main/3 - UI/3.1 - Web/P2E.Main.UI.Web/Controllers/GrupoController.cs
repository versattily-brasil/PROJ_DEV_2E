using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Grupo;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.Main.UI.Web.Extensions.Filters;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class GrupoController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlGrupo;
        #endregion

        #region construtor
        public GrupoController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
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
        [PermissaoFilter("Grupos de Usuários", "Consultar")]
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
                                                                               $"&tx_dsc={vm.descricao}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Grupo>>();
                        vm.DataPage.UrlSearch = $"grupo?";
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
        [PermissaoFilter("Grupos de Usuários", "Editar")]
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

                    CarregarListasComplementares(grupoViewModel);
                    return View("Form", grupoViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [PermissaoFilter("Grupos de Usuários", "Visualizar")]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlGrupo}/{id}");
                    result.EnsureSuccessStatusCode();
                    var grupo = await result.Content.ReadAsAsync<Grupo>();
                    var grupoViewModel = _mapper.Map<GrupoViewModel>(grupo);

                    CarregarListasComplementares(grupoViewModel);
                    return View("View", grupoViewModel);
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
        [PermissaoFilter("Grupos de Usuários", "Criar")]
        public IActionResult Create()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var grupoViewModel = new GrupoViewModel();

                    CarregarListasComplementares(grupoViewModel);
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
        /// <param name="itemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(GrupoViewModel itemViewModel)
        {

            var result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                var grupo = _mapper.Map<Grupo>(itemViewModel);
                                
                if (grupo.IsValid())
                {
                    if (grupo.RotinaGrupoOperacao.Count <= 0)
                    {
                        CarregarListasComplementares(itemViewModel);
                        return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Grupo", "Você precisa associar Rotina ao Grupo"));
                    }

                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlGrupo}/{grupo.CD_GRP}", grupo);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Grupo"));
                    }
                }
                else
                {
                    CarregarListasComplementares(itemViewModel);
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Grupo", grupo.Messages));
                }
            }
            catch (Exception ex)
            {
                CarregarListasComplementares(itemViewModel);
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [PermissaoFilter("Grupos de Usuários", "Deletar")]
        public async Task<IActionResult> Delete(long Id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlGrupo}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();

                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Grupo"));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", new { id = Id }).WithWarning("Erro.", responseBody);
            }            
        }


        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("Grupo")); ;
        }

        #endregion

        /// <summary>
        /// Carrega a lista de rotinas para a tela, dando a possibilidade de ser associada ao grupo atribuindo as permissões "Operações"
        /// </summary>
        /// <returns></returns>
        private async Task<List<RotinaViewModel>> CarregarRotinas()
        {
            string urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/rotina/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlRotina);
                var lista = await result.Content.ReadAsAsync<List<Rotina>>();

                var rotinas = _mapper.Map<List<RotinaViewModel>>(lista);

                //var operacoes = _mapper.Map<List<OperacaoViewModel>>(CarregarOperacoes().Result);

                //foreach (var item in rotinas)
                //{
                //    item.Operacao.AddRange(operacoes);
                //}

                return rotinas;
            }
        }

        /// <summary>
        /// Carregar todas as operações para mostrar em colunas para cada rotina adicionada no grid
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<List<OperacaoViewModel>> CarregarOperacoes()
        {
            string urlOperacao = this.appSettings.ApiBaseURL + $"sso/v1/operacao/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlOperacao);
                var lista = await result.Content.ReadAsAsync<List<Operacao>>();

                return _mapper.Map<List<OperacaoViewModel>>(lista);
            }
        }


        /// <summary>
        /// Carrega a lista de serviços para a tela, dando a possibilidade de filtrar rotinas para associar ao grupo
        /// </summary>
        /// <returns></returns>
        private async Task<List<ServicoViewModel>> CarregarServiços()
        {
            string urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/servico/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlRotina);
                var lista = await result.Content.ReadAsAsync<List<Servico>>();

                var servicos = _mapper.Map<List<ServicoViewModel>>(lista);
                
                return servicos;
            }
        }

        private void CarregarListasComplementares(GrupoViewModel itemViewModel)
        {
            itemViewModel.Operacoes = CarregarOperacoes().Result;
            itemViewModel.Rotinas = CarregarRotinas().Result;
            itemViewModel.Servicos = CarregarServiços().Result;
        }
    }
}