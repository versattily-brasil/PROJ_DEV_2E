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

namespace P2E.Main.UI.Web.Models
{
    public static class NavigationModel
    {
        private const string Underscore = "_";
        private const string Space = " ";
        private static readonly AppSettings appSettings;
        private static string _userId = string.Empty;
        private static ClaimsPrincipal _user;
        public static SmartNavigation Seed => Carregar(null);

        public static SmartNavigation Carregar(ClaimsPrincipal User)
        {

            _user = User;
            //var claims = UserManager.GetClaims(userId);//get claims
            //var someClaim = claims.FirstOrDefault(c => c.Type == "Date")

            var smart = BuildNavigationAsync()?.Result;
            return smart;
        }


        private static async System.Threading.Tasks.Task<SmartNavigation> BuildNavigationAsync(bool seedOnly = true)
        {
            var jsonText = File.ReadAllText("nav.json",System.Text.Encoding.UTF8);
            var navigation = NavigationBuilder.FromJson(jsonText);

            var permissoesJson = _user.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata");

                
                //new List<ServicoViewModel>();



            //var usuarioGrupos = new List<UsuarioGrupo>();
            //var usuarioRotinas = new List<RotinaUsuarioOperacao>();

            // Carregar Dados do DB
            if (!string.IsNullOrEmpty(permissoesJson))
            {
                var servicosViewModel = JsonConvert.DeserializeObject<List<ServicoViewModel>>(permissoesJson);

                //#region Carrega permissões de Usuario x Grupo
                //string urlUsuarioGrupo = $"http://gateway.2e.versattily.com/sso/v1/usuario/permissoesgrupo/{_userId}";
                //using (var client = new HttpClient())
                //{
                //    var result = await client.GetAsync(urlUsuarioGrupo);
                //    usuarioGrupos = await result.Content.ReadAsAsync<List<UsuarioGrupo>>();
                //}

                ////var servicos = new List<Servico>();
                //var servicosViewModel = new List<ServicoViewModel>();

                //// carregar os serviços
                //foreach (var item in usuarioGrupos)
                //{
                //    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                //    {
                //        //if (!servicos.Any(p => p.CD_SRV == subitem.Rotina.Servico.CD_SRV))
                //        if (!servicosViewModel.Any(p => p.CD_SRV == subitem.Rotina.Servico.CD_SRV))
                //            {
                //                var servico = subitem.Rotina.Servico;
                //            //     servicos.Add(servico);

                //            servicosViewModel.Add(new ServicoViewModel()
                //            {
                //                CD_SRV = servico.CD_SRV,
                //                TXT_DEC = servico.TXT_DEC
                //            });
                //        }
                //    }
                //}

                //// carregar as rotinas dos serviços
                //foreach (var item in usuarioGrupos)
                //{
                //    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                //    {
                //        //var servico = servicos.First(p => p.CD_SRV == subitem.Rotina.CD_SRV);
                //        var servico = servicosViewModel.First(p => p.CD_SRV == subitem.Rotina.CD_SRV);

                //        if (servico.RotinasViewModel == null)
                //            servico.RotinasViewModel = new List<RotinaViewModel>();

                //        if (!servico.RotinasViewModel.Any(p => p.CD_ROT == subitem.CD_ROT))
                //        {
                //            var rotinaViewModel = new RotinaViewModel()
                //            {
                //                CD_ROT = subitem.Rotina.CD_ROT,
                //                TX_NOME = subitem.Rotina.TX_NOME,
                //                TX_URL = subitem.Rotina.TX_URL
                //            };

                //            servico.RotinasViewModel.Add(rotinaViewModel);
                //        }
                //    }
                //}
                //#endregion

                //#region Carrega permissões de Usuario x Rotina
                //string urlUsuarioRotina = $"http://gateway.2e.versattily.com/sso/v1/usuario/permissoesusuario/{_userId}";
                //using (var client = new HttpClient())
                //{
                //    var result = await client.GetAsync(urlUsuarioRotina);
                //    usuarioRotinas = await result.Content.ReadAsAsync<List<RotinaUsuarioOperacao>>();
                //}

                //// carregar os serviços
                //foreach (var item in usuarioRotinas)
                //{
                //    if (!servicosViewModel.Any(p => p.CD_SRV == item.Rotina.Servico.CD_SRV))
                //    {
                //        var servico = item.Rotina.Servico;

                //        if (!servicosViewModel.Any(p => p.CD_SRV == servico.CD_SRV))
                //        {
                //            //   servicosViewModel.Add(servico);
                //            servicosViewModel.Add(new ServicoViewModel()
                //            {
                //                CD_SRV = servico.CD_SRV,
                //                TXT_DEC = servico.TXT_DEC
                //            });
                //        }
                //    }
                //}

                //// carregar as rotinas dos serviços
                //foreach (var item in usuarioRotinas)
                //{
                //    var servico = servicosViewModel.First(p => p.CD_SRV == item.Rotina.CD_SRV);

                //    if (servico.RotinasViewModel == null)
                //        servico.RotinasViewModel = new List<RotinaViewModel>();

                //    if (!servico.RotinasViewModel.Any(p => p.CD_ROT == item.CD_ROT))
                //    {
                //        var rotinaViewModel = new RotinaViewModel()
                //        {
                //            CD_ROT = item.Rotina.CD_ROT,
                //            TX_NOME = item.Rotina.TX_NOME,
                //            TX_URL = item.Rotina.TX_URL
                //        };

                //        servico.RotinasViewModel.Add(rotinaViewModel);
                //    }
                //}
                //#endregion

                var listItems = new List<ListItem>();
                foreach (var servico in servicosViewModel.Where(p=> p.RotinasViewModel.Any(x=> x.OperacoesViewModel.Any(q=> q.TX_DSC.Contains("Consultar")))))
                {
                    var item = new ListItem() { Title = servico.TXT_DEC };

                    item.Items = new List<ListItem>();

                    foreach (var rotina in servico.RotinasViewModel.Where(p=> p.OperacoesViewModel.Any(x=> x.TX_DSC.Contains("Consultar"))))
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
