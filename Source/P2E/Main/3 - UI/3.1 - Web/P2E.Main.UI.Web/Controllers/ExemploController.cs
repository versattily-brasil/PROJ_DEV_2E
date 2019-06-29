using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.API.ViewModel;

namespace P2E.Main.UI.Web.Controllers
{
    public class ExemploController : Controller
    {
        [HttpGet]
        public IActionResult Lista()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://localhost:50761/api/v1/exemplo").Result;

            List<ExemploVM> list = new List<ExemploVM>();

            if (result.IsSuccessStatusCode)
            {
                list = result.Content.ReadAsAsync<List<ExemploVM>>().Result;
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Novo(ExemploVM exemplo)
        {
            HttpClient client = new HttpClient();
            client.PostAsJsonAsync<ExemploVM>("http://localhost:50761/api/v1/exemplo", exemplo);

            return RedirectToAction("Lista");
        }
    }
}