using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.SSO.API.ViewModel;

namespace P2E.Main.UI.Web.Controllers
{
    public class RotinaController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlRotina;
        #endregion

        public RotinaController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/rotina";
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(_urlRotina);
            result.EnsureSuccessStatusCode();
            List<RotinaVM> list = await result.Content.ReadAsAsync<List<RotinaVM>>();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int id)
        {
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(_urlRotina + "/" + id);
                result.EnsureSuccessStatusCode();

                RotinaVM rotina = await result.Content.ReadAsAsync<RotinaVM>();

                return View(rotina);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(RotinaVM rotina)
        {
            HttpClient client = new HttpClient();
            await client.PutAsJsonAsync<RotinaVM>(_urlRotina + "/" + rotina.CD_ROT, rotina);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "A Rotina foi salva corretamente.");
        }

        public async Task<IActionResult> Excluir(int Id)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(_urlRotina + "/" + Id);
            return RedirectToAction("Lista").WithSuccess("Sucesso.", "A Rotina foi excluída corretamente.");
        }
    }
}