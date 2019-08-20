using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.UI.Web.Extensions.Alerts;
using P2E.Main.UI.Web.Models;
using P2E.Main.UI.Web.Models.SSO.Usuario;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Shared.Message;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Main.UI.Web.Models.SSO.Servico;
using Newtonsoft.Json;
using P2E.Main.UI.Web.Extensions.Util;

namespace P2E.Main.UI.Web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        #region variáveis locais
        private readonly AppSettings appSettings;
        private readonly IMapper _mapper;
        private string _urlUsuario;
        private string _urlModulo;
        private string _urlGrupo;
        #endregion

        public UsuarioController(AppSettings appSettings, IMapper mapper)
        {
            this.appSettings = appSettings;
            _mapper = mapper;
            _urlUsuario = this?.appSettings?.ApiBaseURL + $"sso/v1/usuario";
            _urlModulo = this?.appSettings?.ApiBaseURL + $"sso/v1/modulo";
            _urlGrupo = this?.appSettings?.ApiBaseURL + $"sso/v1/grupo";
        }

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPage"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(UsuarioListViewModel vm)
        {
            try
            {
                if (vm.DataPage.CurrentPage > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync($"{_urlUsuario}" +
                                                                               $"?currentpage={vm.DataPage.CurrentPage}" +
                                                                               $"&pagesize={vm.DataPage.PageSize}" +
                                                                               $"&orderby={vm.DataPage.OrderBy}" +
                                                                               $"&Descending={vm.DataPage.Descending}" +
                                                                               $"&tx_nome={vm.TX_NOME}");
                        result.EnsureSuccessStatusCode();
                        vm.DataPage = await result.Content.ReadAsAsync<DataPage<Usuario>>();
                        vm.DataPage.UrlSearch = $"usuario?";
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
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlUsuario}/{id}");
                    result.EnsureSuccessStatusCode();
                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);
                    CarregarListasComplementares(usuarioViewModel);
                    return View("Form", usuarioViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlUsuario}/{id}");
                    result.EnsureSuccessStatusCode();
                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);
                    CarregarListasComplementares(usuarioViewModel);
                    return View("View", usuarioViewModel);
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
        public async Task<IActionResult> Create(long id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync($"{_urlUsuario}/{id}");
                    result.EnsureSuccessStatusCode();
                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModel = _mapper.Map<UsuarioViewModel>(usuario);
                    CarregarListasComplementares(usuarioViewModel);
                    return View("Form", usuarioViewModel);
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
        /// <param name="itemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(UsuarioViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;
            var usuario = _mapper.Map<Usuario>(itemViewModel);
            
            try
            {
                if (usuario.CONFIRMA_SENHA != null)
                {
                    if (usuario.TX_SENHA.Trim() != usuario.CONFIRMA_SENHA.Trim())
                    {
                        using (var client = new HttpClient())
                        {
                            var results = await client.GetAsync($"{_urlUsuario}/{0}");
                            results.EnsureSuccessStatusCode();
                            var usuarios = await results.Content.ReadAsAsync<Usuario>();
                            var usuarioViewModels = _mapper.Map<UsuarioViewModel>(usuarios);

                            itemViewModel.Modulo = usuarioViewModels.Modulo;
                            itemViewModel.Grupo = usuarioViewModels.Grupo;
                        }
                        CarregarListasComplementares(itemViewModel);

                        return View("Form", itemViewModel).WithDanger("Senha.", GenericMessages.ErrorComparePassword("Usuario", usuario.Messages));
                    }
                }

                if (usuario.IsValid())
                {
                    using (var client = new HttpClient())
                    {
                        result = await client.PutAsJsonAsync($"{_urlUsuario}/{usuario.CD_USR}", usuario);
                        responseBody = await result.Content.ReadAsStringAsync();
                        result.EnsureSuccessStatusCode();
                        return RedirectToAction("Index").WithSuccess("Sucesso", GenericMessages.SucessSave("Usuario"));
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var results = await client.GetAsync($"{_urlUsuario}/{0}");
                        results.EnsureSuccessStatusCode();
                        var usuarios = await results.Content.ReadAsAsync<Usuario>();
                        var usuarioViewModels = _mapper.Map<UsuarioViewModel>(usuarios);

                        itemViewModel.Modulo = usuarioViewModels.Modulo;
                        itemViewModel.Grupo = usuarioViewModels.Grupo;
                    }
                    CarregarListasComplementares(itemViewModel);
                    return View("Form", itemViewModel).WithDanger("Erro.", GenericMessages.ErrorSave("Usuario", usuario.Messages));
                }
            }
            catch (Exception ex)
            {
                using (var client = new HttpClient())
                {
                    var results = await client.GetAsync($"{_urlUsuario}/{0}");
                    results.EnsureSuccessStatusCode();
                    var usuarios = await results.Content.ReadAsAsync<Usuario>();
                    var usuarioViewModels = _mapper.Map<UsuarioViewModel>(usuarios);

                    itemViewModel.Modulo = usuarioViewModels.Modulo;
                    itemViewModel.Grupo = usuarioViewModels.Grupo;
                }
                CarregarListasComplementares(itemViewModel);
                return View("Form", itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long Id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    result = await client.DeleteAsync($"{_urlUsuario}/{Id}");
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();

                    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Usuario"));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", new { id = Id }).WithWarning("Erro.", responseBody);
            }

            //using (var client = new HttpClient())
            //{
            //    await client.DeleteAsync($"{_urlUsuario}/{Id}");
            //    return RedirectToAction("Index").WithSuccess("Sucesso.", GenericMessages.SucessRemove("Usuario"));
            //}
        }

        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction("Index").WithSuccess("Cancelada.", GenericMessages.EditCancel("Usuário")); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioViewModel itemViewModel)
        {
            var result = new HttpResponseMessage();
            string responseBody = string.Empty;

            try
            {
                var usuarioLogin = _mapper.Map<Usuario>(itemViewModel);
                using (var client = new HttpClient())
                {
                    result = await client.PostAsJsonAsync($"{_urlUsuario}/login", usuarioLogin);
                    responseBody = await result.Content.ReadAsStringAsync();
                    result.EnsureSuccessStatusCode();

                    var usuario = await result.Content.ReadAsAsync<Usuario>();
                    if(usuario != null)
                    {
                        if (usuario.OP_STATUS.ToString().Trim() == "INATIVO")
                        {
                            return View(itemViewModel).WithDanger("Acesso Negado", "Por favor contacte o administrador do sistema.");
                        }

                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, usuario.TX_LOGIN));
                        identity.AddClaim(new Claim(ClaimTypes.Sid, usuario.CD_USR.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Uri, this.appSettings.ApiBaseURL));

                        var permissoes = CarregarPermissoesAsync(usuario.CD_USR).Result;
                        identity.AddClaim(new Claim("permissoes", permissoes));

                        var principal = new ClaimsPrincipal(identity);

                        var prop = new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(2)
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, prop);


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(itemViewModel).WithDanger("Erro.", "Usuário ou Senha inválidos");
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                return View( itemViewModel).WithDanger("Erro", responseBody);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }


        /// <summary>
        /// Carrega a lista de rotinas para a tela, dando a possibilidade de ser associada ao grupo atribuindo as permissões "Operações"
        /// </summary>
        /// <returns></returns>
        private async Task<List<RotinaViewModel>> CarregarRotinas()
        {
            string urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/rotina/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlRotina);
                var lista = await result.Content.ReadAsAsync<List<Rotina>>();

                var rotinas = _mapper.Map<List<RotinaViewModel>>(lista);
                
                return rotinas;
            }
        }

        /// <summary>
        /// Carregar todas as operações para mostrar em colunas para cada rotina adicionada no grid
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<List<OperacaoViewModel>> CarregarOperacoes()
        {
            string urlOperacao = this.appSettings.ApiBaseURL + $"sso/v1/operacao/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlOperacao);
                var lista = await result.Content.ReadAsAsync<List<Operacao>>();

                return _mapper.Map<List<OperacaoViewModel>>(lista);
            }
        }


        /// <summary>
        /// Carrega a lista de serviços para a tela, dando a possibilidade de filtrar rotinas para associar ao grupo
        /// </summary>
        /// <returns></returns>
        private async Task<List<ServicoViewModel>> CarregarServiços()
        {
            string urlRotina = this.appSettings.ApiBaseURL + $"sso/v1/servico/todos";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(urlRotina);
                var lista = await result.Content.ReadAsAsync<List<Servico>>();

                var servicos = _mapper.Map<List<ServicoViewModel>>(lista);

                return servicos;
            }
        }

        private void CarregarListasComplementares(UsuarioViewModel itemViewModel)
        {
            itemViewModel.Operacoes = CarregarOperacoes().Result;
            itemViewModel.Rotinas = CarregarRotinas().Result;
            itemViewModel.Servicos = CarregarServiços().Result;
        }

        #endregion

        public async Task<string> CarregarPermissoesAsync(int usuarioId)
        {
            List<UsuarioGrupo> usuarioGrupos = new List<UsuarioGrupo>();
            var usuarioRotinas = new List<RotinaUsuarioOperacao>();

            #region Carrega permissões de Usuario x Grupo
            string urlUsuarioGrupo = $"{_urlUsuario}/permissoesgrupo/{usuarioId}";
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
            string urlUsuarioRotina = $"{_urlUsuario}/permissoesusuario/{usuarioId}";
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
    }
}