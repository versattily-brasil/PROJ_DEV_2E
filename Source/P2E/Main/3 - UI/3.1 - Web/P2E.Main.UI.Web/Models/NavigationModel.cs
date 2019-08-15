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

namespace P2E.Main.UI.Web.Models
{
    public static class NavigationModel
    {
        private const string Underscore = "_";
        private const string Space = " ";
        private static readonly AppSettings appSettings;
        private static string _userId = string.Empty;

        public static SmartNavigation Seed => Carregar(null);

        public static SmartNavigation Carregar(ClaimsPrincipal User)
        {

            _userId = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid");

            //var claims = UserManager.GetClaims(userId);//get claims
            //var someClaim = claims.FirstOrDefault(c => c.Type == "Date")

            var smart = BuildNavigationAsync()?.Result;
            return smart;
        }


        private static async System.Threading.Tasks.Task<SmartNavigation> BuildNavigationAsync(bool seedOnly = true)
        {
            var jsonText = File.ReadAllText("nav.json",System.Text.Encoding.UTF8);
            var navigation = NavigationBuilder.FromJson(jsonText);

            var usuarioGrupos = new List<UsuarioGrupo>();

            // Carregar Dados do DB

            if (!string.IsNullOrEmpty(_userId))
            {
                string urlUsuario = $"http://localhost:7000/sso/v1/usuario/permissoes/{_userId}";
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(urlUsuario);
                    usuarioGrupos = await result.Content.ReadAsAsync<List<UsuarioGrupo>>();
                }


                var servicos = new List<Servico>();

                // carregar os serviços
                foreach (var item in usuarioGrupos)
                {
                    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                    {
                        if (!servicos.Any(p => p.CD_SRV == subitem.Rotina.Servico.CD_SRV))
                        {
                            var servico = subitem.Rotina.Servico;
                            servicos.Add(servico);
                        }
                    }
                }


                // carregar as rotinas dos serviços
                // carregar os serviços
                foreach (var item in usuarioGrupos)
                {
                    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                    {
                        var servico = servicos.First(p=> p.CD_SRV == subitem.Rotina.CD_SRV);

                        if (servico.Rotinas == null)
                            servico.Rotinas = new List<Rotina>();

                        if (!servico.Rotinas.Any(p => p.CD_ROT == subitem.CD_ROT))
                        {
                            servico.Rotinas.Add(subitem.Rotina);
                        }
                    }
                }

                //foreach (var item in usuarioGrupos)
                //{
                //    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                //    {
                //        if (!servicos.Any(p => p.CD_SRV == subitem.Rotina.Servico.CD_SRV))
                //        {
                //            var servico = subitem.Rotina.Servico;

                //            servico.Rotinas = new List<Rotina>();

                //            var permissoes = item.ListaRotinaGrupoOperacao
                //                .Where(p => p.Rotina.CD_SRV == servico.CD_SRV);

                //            foreach (var permissao in permissoes)
                //            {
                //                servico.Rotinas.Add(permissao.Rotina);
                //            }

                //            servicos.Add(servico);
                //        }
                //    }

                //}

                //for (int i = 0; i < servicos.Count; i++)
                //{
                //    var root = servicos[i].Rotinas;
                //    servicos[i].Rotinas = new List<Rotina>();

                //    foreach (var r in root)
                //    {
                //        if (!servicos[i].Rotinas.Any(p => p.CD_ROT == r.CD_ROT))
                //        {
                //            servicos[i].Rotinas.Add(r);
                //        }
                //    }
                //}

                var listItems = new List<ListItem>();


                foreach (var servico in servicos.Distinct())
                {
                    var item = new ListItem() { Title = servico.TXT_DEC };

                    item.Items = new List<ListItem>();

                    foreach (var rotina in servico.Rotinas)
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
                //var menu = FillProperties(navigation.Lists, seedOnly);

                return new SmartNavigation(menu);
            }
            else
            {
                return new SmartNavigation(null);
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
