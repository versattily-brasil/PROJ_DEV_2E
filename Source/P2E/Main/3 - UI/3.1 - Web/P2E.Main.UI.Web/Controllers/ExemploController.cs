using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P2E.Main.API.ViewModel;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;

namespace P2E.Main.UI.Web.Controllers
{
    public class ExemploController : Controller
    {
        private readonly AppSettings appSettings;

        public ExemploController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL+"/exemplo");
            result.EnsureSuccessStatusCode();
            List<ExemploVM> list = await result.Content.ReadAsAsync<List<ExemploVM>>();
            
            return View(list);
        }

        [HttpGet]
        public async Task<List<ExemploVM>> Listar()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/exemplo");
            result.EnsureSuccessStatusCode();
            List<ExemploVM> list = await result.Content.ReadAsAsync<List<ExemploVM>>();

            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(string values)
        {
            var exemplo = new ExemploVM();
            JsonConvert.PopulateObject(values, exemplo);

            HttpClient client = new HttpClient();
            await client.PostAsJsonAsync<ExemploVM>(this.appSettings.ApiBaseURL + "/exemplo/" , exemplo);
            return Ok(exemplo);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(int key, string values)
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(this.appSettings.ApiBaseURL + "/exemplo/"+key);
            result.EnsureSuccessStatusCode();

            var exemplo = await result.Content.ReadAsAsync<ExemploVM>();
            JsonConvert.PopulateObject(values, exemplo);
            await client.PutAsJsonAsync<ExemploVM>(this.appSettings.ApiBaseURL + "/exemplo/" + key, exemplo);
            return Ok(exemplo);
        }

        [HttpDelete]
        public async Task Excluir(int key)
        {
            HttpClient client = new HttpClient();
            await client.DeleteAsync(this.appSettings.ApiBaseURL + "/exemplo/" + key);
        }
    }
}