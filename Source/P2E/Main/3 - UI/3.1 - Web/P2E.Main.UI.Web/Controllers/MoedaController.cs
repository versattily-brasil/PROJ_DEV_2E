using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Extensions.Filters;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.IMP.Moeda;
using P2E.Shared.Message;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class MoedaController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlMoeda;
        #endregion

        #region construtor
        public MoedaController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlMoeda = this.appSettings.ApiBaseURL + $"imp/v1/moeda";
        }
        #endregion


        #region Métodos

        [HttpPost]
        public async Task<IActionResult> Upload(string caminhoXML)
        {
            if (!System.IO.Directory.Exists(caminhoXML))
            {
                Moeda moeda = new Moeda();

                //XElement xml = XElement.Load(arquivoPath);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(caminhoXML);

                //Pegando elemento pelo nome da TAG
                XmlNodeList xnList = xmlDoc.GetElementsByTagName("Moeda");

                var result = new HttpResponseMessage();
                string responseBody = string.Empty;

                using (var client = new HttpClient())
                {
                    //Usando for para imprimir na tela
                    for (int i = 0; i < xnList.Count; i++)
                    {
                        string codigo = xnList[i]["codigo"].InnerText;
                        string descricao = xnList[i]["descricao"].InnerText;

                        moeda.TX_COD_IMP = codigo;
                        moeda.TX_DESCRICAO = descricao;
                        moeda.TX_COD_EXP = "000";

                        result = await client.PutAsJsonAsync($"{_urlMoeda}/{0}", moeda);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();                        
                    }
                }
            }

            return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Moeda"));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissaoFilter("Moeda", "Consultar")]
        public async Task<IActionResult> Index(MoedaListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlMoeda}" +
                                                           $"?currentpage={vm.DataPage.CurrentPage}" +
                                                           $"&pagesize={vm.DataPage.PageSize}" +
                                                           $"&orderby={vm.DataPage.OrderBy}" +
                                                           $"&Descending={vm.DataPage.Descending}" +
                                                           $"&TX_DESCRICAO={vm.descricao}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Moeda>>();
                        vm.DataPage.UrlSearch = $"moeda?";
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
        [PermissaoFilter("Moeda", "Editar")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlMoeda}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<Moeda>();
                    var moduloViewModel = _mapper.Map<MoedaViewModel>(modulo);
                    return View("Form", moduloViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [PermissaoFilter("Moeda", "Visualizar")]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlMoeda}/{id}");
                    result.EnsureSuccessStatusCode();
                    var modulo = await result.Content.ReadAsAsync<Moeda>();
                    var moduloViewModel = _mapper.Map<MoedaViewModel>(modulo);
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
        [PermissaoFilter("Moeda", "Criar")]
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
        public async Task<IActionResult> Save(MoedaViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            try
            {
                var modulo = _mapper.Map<Moeda>(itemViewModel);
                //if (modulo.IsValid())
                //{
                using (var client = new HttpClient())
                {
                    result = await client.PutAsJsonAsync($"{_urlMoeda}/{modulo.CD_MOEDA}", modulo);
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();
                    return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Moeda"));
                }
                //}
                //else
                //{
                //    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("moeda", modulo.Messages));
                //}
            }
            catch (Exception ex)
            {
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        public IActionResult CancelInsert()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.InsertCancel("Moeda"));
        }

        public IActionResult CancelEdit()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("Moeda"));
        }
        public IActionResult CancelView()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.ShowDetailCancel("Moeda"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [PermissaoFilter("Moeda", "Deletar")]
        public async Task<IActionResult> Delete(long Id)
        {
            string responseBody = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage();

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlMoeda}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();
                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Moeda"));
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
