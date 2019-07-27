using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
        #endregion

        #region construtor
        public ParceiroNegocioController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
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
        public async Task<IActionResult> Index(DataPage<ParceiroNegocio> dataPage, string razaosocial, string cnpj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlParceiro}?currentPage{dataPage.CurrentPage}&pagesize={dataPage.PageSize}&orderby={dataPage.OrderBy}&Descending={dataPage.Descending}");
                    result.EnsureSuccessStatusCode();
                    dataPage = await result.Content.ReadAsAsync<DataPage<ParceiroNegocio>>();
                    return View(dataPage);
                }
            }
            catch (Exception ex)
            {
                dataPage.Message = ex.Message;
                return View(dataPage);
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
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{_urlParceiro}/{id}");
                var parceiroNegocio = _mapper.Map<ParceiroNegocioViewModel>(result);
                return View(parceiroNegocio);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
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
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Parceiro Negócio"));
                    }
                }
                else
                {
                    return View(itemViewModel.CD_PAR > 0 ? "Edit" : "Create", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Parceiro Negocio", parceiroNegocio.Messages));
                }
            }
            catch (Exception ex)
            {
                return View(itemViewModel.CD_PAR > 0 ? "Edit" : "Create", itemViewModel).WithDanger("Erro", ex.Message);
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