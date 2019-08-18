using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.API.ViewModel;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class ServicoController : Controller
    {

        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlServico;
        #endregion

        #region construtor
        public ServicoController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlServico = this.appSettings.ApiBaseURL + $"sso/v1/servico";
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
        public async Task<IActionResult> Index(ServicoListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlServico}" +
                                                           $"?currentpage={vm.DataPage.CurrentPage}" +
                                                           $"&pagesize={vm.DataPage.PageSize}" +
                                                           $"&orderby={vm.DataPage.OrderBy}" +
                                                           $"&Descending={vm.DataPage.Descending}" +
                                                           $"&TXT_DEC={vm.TXT_DEC}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Servico>>();
                        vm.DataPage.UrlSearch = $"servico?";
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
                    var result = await client.GetAsync($"{_urlServico}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<Servico>();
                    var moduloViewModel = _mapper.Map<ServicoViewModel>(modulo);
                    return View("Form", moduloViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlServico}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<Servico>();
                    var moduloViewModel = _mapper.Map<ServicoViewModel>(modulo);
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
        public async Task<IActionResult> Save(ServicoViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var modulo = _mapper.Map<Servico>(itemViewModel);
                if (modulo.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlServico}/{modulo.CD_SRV}", modulo);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Serviço"));
                    }
                }
                else
                {
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Serviço", modulo.Messages));
                }
            }
            catch (Exception ex)
            {
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("Serviço")); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long Id)
        {
            string responseBody = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage();

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlServico}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();
                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Serviço"));
                }

            }
            catch (Exception)
            {
                return RedirectToAction("View", new { id = Id }).WithWarning("Erro.", responseBody);
            }           
        }
#endregion

        #region old code
        //[HttpGet]
        //public async Task<IActionResult> Lista()
        //{
        //    HttpClient client = new HttpClient();
        //    var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/servico");
        //    result.EnsureSuccessStatusCode();
        //    List<ServicoVM> list = await result.Content.ReadAsAsync<List<ServicoVM>>();

        //    return View(list);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Cadastro(int id)
        //{
        //    if (id != 0)
        //    {
        //        HttpClient client = new HttpClient();
        //        var result = await client.GetAsync(this.appSettings.ApiUsuarioBaseURL + "/servico/" + id);
        //        result.EnsureSuccessStatusCode();

        //        ServicoVM exemplo = await result.Content.ReadAsAsync<ServicoVM>();

        //        return View(exemplo);
        //    }

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Cadastro(ServicoVM exemplo)
        //{
        //    if (exemplo.TXT_DEC == String.Empty)
        //    {
        //        return View(exemplo).WithDanger("Erro.", "Preencha todos os campos.");
        //    }

        //    HttpClient client = new HttpClient();
        //    await client.PutAsJsonAsync<ServicoVM>(this.appSettings.ApiUsuarioBaseURL + "/servico/" + exemplo.CD_SRV, exemplo);
        //    return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Exemplo foi salvo corretamente.");
        //}

        //public async Task<IActionResult> Excluir(int Id)
        //{
        //    HttpClient client = new HttpClient();
        //    await client.DeleteAsync(this.appSettings.ApiUsuarioBaseURL + "/servico/" + Id);
        //    return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Exemplo foi excluído corretamente.");

        //} 
        #endregion
    }
}
