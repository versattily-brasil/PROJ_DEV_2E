using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.ParceiroNegocio;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Controllers
{
    public class ParceiroNegocioController : Controller
    {
        private readonly AppSettings appSettings;

        public ParceiroNegocioController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DataPage<ParceiroNegocio> dataPage, string razaosocial, string cnpj)
        {
            using (var client = new HttpClient())
            {
                string url = $"/sso/v1/parceironegocio?currentPage{dataPage.CurrentPage}&pagesize={dataPage.PageSize}&orderby={dataPage.OrderBy}&Descending={dataPage.Descending}";
                var result = await client.GetAsync(this.appSettings.ApiBaseURL + url);
                result.EnsureSuccessStatusCode();
                dataPage = await result.Content.ReadAsAsync<DataPage<ParceiroNegocio>>();
                return View(dataPage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(ParceiroNegocioViewModel item)
        {
            //HttpClient client = new HttpClient();
            //await client.PutAsJsonAsync<item>(this.appSettings.ApiUsuarioBaseURL + "/usuario/" + usuario.CD_USR, usuario);
            return RedirectToAction("Index").WithSuccess("Sucesso.", "O Usuário foi salvo corretamente.");
        }

        //public async Task<IActionResult> Excluir(int Id)
        //{
        //    HttpClient client = new HttpClient();
        //    await client.DeleteAsync(this.appSettings.ApiUsuarioBaseURL + "/usuario/" + Id);
        //    return RedirectToAction("Lista").WithSuccess("Sucesso.", "O Usuário foi excluído corretamente.");

        //}
    }
}