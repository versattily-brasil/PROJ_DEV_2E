using P2E.SSO.Domain.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.ParceiroNegocio;
using P2E.Shared.Message;
using P2E.Shared.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Web;
using Microsoft.AspNetCore.Identity;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using Newtonsoft.Json;
using P2E.Main.UI.Web.Extensions.Util;
using P2E.Main.UI.Web.Controllers;

namespace P2E.Main.UI.Web.Models
{
    public static class NavigationModel
    {
        private const string Underscore = "_";
        private const string Space = " ";
        private static string _userId = string.Empty;
        private static ClaimsPrincipal _principal;
        public static SmartNavigation Seed => Carregar(null);

        public static SmartNavigation Carregar(ClaimsPrincipal principal)
        {

            _principal = principal;
            var smart = BuildNavigation();
            return smart;
        }


        private static SmartNavigation BuildNavigation(bool seedOnly = true)
        {
            try
            {
                var permissoesJson = Permissoes.CarregarPermissoesAsync(_principal).Result;


                // Carregar Dados do DB
                if (!string.IsNullOrEmpty(permissoesJson))
                {
                    var servicosViewModel = JsonConvert.DeserializeObject<List<ServicoViewModel>>(permissoesJson);

                    var listItems = new List<ListItem>();
                    foreach (var servico in servicosViewModel.Where(p => p.RotinasViewModel.Any(x => x.OperacoesViewModel.Any(q => q.TX_DSC.Contains("Consultar")))))
                    {
                        var item = new ListItem() { Title = servico.TXT_DEC };

                        item.Items = new List<ListItem>();

                        foreach (var rotina in servico.RotinasViewModel.Where(p => p.OperacoesViewModel.Any(x => x.TX_DSC.Contains("Consultar"))))
                        {
                            item.Items.Add(new ListItem()
                            {
                                Title = rotina.TX_NOME,
                                Href = rotina.TX_URL
                            });
                        }

                        listItems.Add(item);
                    }

                    var menu = FillProperties(listItems, seedOnly);
                    return new SmartNavigation(menu);
                }
                else
                {
                    return new SmartNavigation(new List<ListItem>());
                }
            }
            catch (System.Exception)
            {
                return new SmartNavigation(new List<ListItem>());
            }
        }

        private static List<ListItem> FillProperties(IEnumerable<ListItem> items, bool seedOnly, ListItem parent = null)
        {
            var result = new List<ListItem>();

            foreach (var item in items)
            {
                item.Text = item.Text ?? item.Title;
                item.Tags = string.Concat(parent?.Tags, Space, item.Title.ToLower()).Trim();

                var route = Path.GetFileNameWithoutExtension(item.Href ?? string.Empty)?.Split(Underscore);

                item.Route = route?.Length > 1 ? $"/{route.First()}/{string.Join(string.Empty, route.Skip(1))}" : item.Href;

                item.I18n = parent == null
                    ? $"nav.{item.Title.ToLower().Replace(Space, Underscore)}"
                    : $"{parent.I18n}_{item.Title.ToLower().Replace(Space, Underscore)}";

                item.Items = FillProperties(item.Items, seedOnly, item);

                if (!seedOnly || item.ShowOnSeed)
                    result.Add(item);
            }

            return result;
        }
    }
}
