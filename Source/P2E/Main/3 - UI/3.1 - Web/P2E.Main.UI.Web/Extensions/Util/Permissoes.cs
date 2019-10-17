using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Extensions.Util
{
    public static class Permissoes
    {
        public static ConfigurationRoot Configuration { get; set; }

        public static bool VerificarPermissao(string rotina, string operacao, ClaimsPrincipal principal)
        {
            try
            {
                var permissoesJson = principal.FindFirstValue("permissoes");
                var servicosViewModel = JsonConvert.DeserializeObject<List<ServicoViewModel>>(permissoesJson);

                if (servicosViewModel.Any(p => p.RotinasViewModel.Any(x => x.TX_NOME.Equals(rotina) && x.OperacoesViewModel.Any(o => o.TX_DSC.Contains(operacao)))))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static async Task<string> CarregarPermissoesAsync(ClaimsPrincipal principal)
        {
            try
            {
                var identity = principal.Identities.FirstOrDefault();

                var usuarioId = identity.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid");

                string urlBase = principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri");
                string urlUsuario = urlBase + $"sso/v1/usuario";
                string urlRotina = urlBase + $"sso/v1/rotina";

                var claimPermissoes = identity.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2008/06/identity/claims/userdata");

                if (claimPermissoes != null)
                {
                    identity.TryRemoveClaim(claimPermissoes);
                }

                List<UsuarioGrupo> usuarioGrupos = new List<UsuarioGrupo>();
                var usuarioRotinas = new List<RotinaUsuarioOperacao>();

                #region Carrega permissões de Usuario x Grupo
                string urlUsuarioGrupo = $"{urlUsuario}/permissoesgrupo/{usuarioId.Value}";
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(urlUsuarioGrupo);
                    usuarioGrupos = await result.Content.ReadAsAsync<List<UsuarioGrupo>>();
                }

                var servicosViewModel = new List<ServicoViewModel>();

                // carregar os serviços
                foreach (var item in usuarioGrupos)
                {
                    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                    {
                        if (!servicosViewModel.Any(p => p.CD_SRV == subitem.Rotina.Servico.CD_SRV))
                        {
                            var servico = subitem.Rotina.Servico;
                            servicosViewModel.Add(new ServicoViewModel()
                            {
                                CD_SRV = servico.CD_SRV,
                                TXT_DEC = servico.TXT_DEC
                            });
                        }
                    }
                }

                // carregar as rotinas dos serviços
                foreach (var item in usuarioGrupos)
                {
                    foreach (var subitem in item.ListaRotinaGrupoOperacao)
                    {
                        var servico = servicosViewModel.First(p => p.CD_SRV == subitem.Rotina.CD_SRV);

                        if (servico.RotinasViewModel == null)
                            servico.RotinasViewModel = new List<RotinaViewModel>();

                        if (!servico.RotinasViewModel.Any(p => p.CD_ROT == subitem.CD_ROT))
                        {
                            var rotinaViewModel = new RotinaViewModel()
                            {
                                CD_ROT = subitem.Rotina.CD_ROT,
                                TX_NOME = subitem.Rotina.TX_NOME,
                                TX_URL = subitem.Rotina.TX_URL
                            };

                            string url = $"{urlRotina}/associadas/{subitem.CD_ROT}";

                            using (var client = new HttpClient())
                            {
                                var result = client.GetAsync(url).Result;
                                var lista = await result.Content.ReadAsAsync<List<RotinaAssociada>>();
                                rotinaViewModel.RotinasAssociadas = lista;
                            }


                            if (rotinaViewModel.OperacoesViewModel == null)
                            {
                                rotinaViewModel.OperacoesViewModel = new List<OperacaoViewModel>();
                            }

                            if (!rotinaViewModel.OperacoesViewModel.Any(p => p.CD_OPR == subitem.CD_OPR))
                            {
                                rotinaViewModel.OperacoesViewModel.Add(new OperacaoViewModel()
                                {
                                    CD_OPR = subitem.CD_OPR,
                                    TX_DSC = subitem.Operacao.TX_DSC
                                });
                            }

                            servico.RotinasViewModel.Add(rotinaViewModel);
                        }
                        else
                        {
                            var rotinaViewModel = servico.RotinasViewModel.FirstOrDefault(p => p.CD_ROT == subitem.CD_ROT);

                            if (rotinaViewModel.OperacoesViewModel == null)
                            {
                                rotinaViewModel.OperacoesViewModel = new List<OperacaoViewModel>();
                            }

                            if (!rotinaViewModel.OperacoesViewModel.Any(p => p.CD_OPR == subitem.CD_OPR))
                            {
                                rotinaViewModel.OperacoesViewModel.Add(new OperacaoViewModel()
                                {
                                    CD_OPR = subitem.CD_OPR,
                                    TX_DSC = subitem.Operacao.TX_DSC
                                });
                            }

                        }
                    }
                }
                #endregion

                #region Carrega permissões de Usuario x Rotina
                string urlUsuarioRotina = $"{urlUsuario}/permissoesusuario/{usuarioId.Value}";
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(urlUsuarioRotina);
                    usuarioRotinas = await result.Content.ReadAsAsync<List<RotinaUsuarioOperacao>>();
                }

                // carregar os serviços
                foreach (var item in usuarioRotinas)
                {
                    if (!servicosViewModel.Any(p => p.CD_SRV == item.Rotina.Servico.CD_SRV))
                    {
                        var servico = item.Rotina.Servico;

                        if (!servicosViewModel.Any(p => p.CD_SRV == servico.CD_SRV))
                        {
                            servicosViewModel.Add(new ServicoViewModel()
                            {
                                CD_SRV = servico.CD_SRV,
                                TXT_DEC = servico.TXT_DEC
                            });
                        }
                    }
                }

                // carregar as rotinas dos serviços
                foreach (var item in usuarioRotinas)
                {
                    var servico = servicosViewModel.First(p => p.CD_SRV == item.Rotina.CD_SRV);

                    if (servico.RotinasViewModel == null)
                        servico.RotinasViewModel = new List<RotinaViewModel>();

                    if (!servico.RotinasViewModel.Any(p => p.CD_ROT == item.CD_ROT))
                    {
                        var rotinaViewModel = new RotinaViewModel()
                        {
                            CD_ROT = item.Rotina.CD_ROT,
                            TX_NOME = item.Rotina.TX_NOME,
                            TX_URL = item.Rotina.TX_URL
                        };

                        if (rotinaViewModel.OperacoesViewModel == null)
                        {
                            rotinaViewModel.OperacoesViewModel = new List<OperacaoViewModel>();
                        }

                        if (!rotinaViewModel.OperacoesViewModel.Any(p => p.CD_OPR == item.CD_OPR))
                        {
                            rotinaViewModel.OperacoesViewModel.Add(new OperacaoViewModel()
                            {
                                CD_OPR = item.CD_OPR,
                                TX_DSC = item.Operacao.TX_DSC
                            });
                        }

                        servico.RotinasViewModel.Add(rotinaViewModel);
                    }
                    else
                    {
                        var rotinaViewModel = servico.RotinasViewModel.FirstOrDefault(p => p.CD_ROT == item.CD_ROT);

                        if (rotinaViewModel.OperacoesViewModel == null)
                        {
                            rotinaViewModel.OperacoesViewModel = new List<OperacaoViewModel>();
                        }

                        if (!rotinaViewModel.OperacoesViewModel.Any(p => p.CD_OPR == item.CD_OPR))
                        {
                            rotinaViewModel.OperacoesViewModel.Add(new OperacaoViewModel()
                            {
                                CD_OPR = item.CD_OPR,
                                TX_DSC = item.Operacao.TX_DSC
                            });
                        }

                    }
                }
                #endregion

                var permissoes = JsonConvert.SerializeObject(servicosViewModel);
                return permissoes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
