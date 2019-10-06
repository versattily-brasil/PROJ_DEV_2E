using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using P2E.Main.UI.Web.Models.SSO.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace P2E.Main.UI.Web.Extensions.Filters
{
    public class PermissaoFilterAttribute : TypeFilterAttribute
    {

        public PermissaoFilterAttribute(string rotina, string operacao) : base(typeof(ClaimRequirementFilter))
        {
           Arguments = new object[] { rotina, operacao };
        }

        public class ClaimRequirementFilter : IAuthorizationFilter
        {
            readonly string _rotina;
            readonly string _operacao;

            public ClaimRequirementFilter(string rotina, string operacao)
            {
                _rotina = rotina;
                _operacao = operacao;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                try
                {
                    var principal = context.HttpContext.User;

                    var permissoesJson = principal.FindFirstValue("permissoes");
                    var servicosViewModel = JsonConvert.DeserializeObject<List<ServicoViewModel>>(permissoesJson);

                    if (servicosViewModel.Any(p => p.RotinasViewModel.Any(x => x.TX_NOME.Equals(_rotina) && x.OperacoesViewModel.Any(o => o.TX_DSC.Contains(_operacao)))))
                    {
                        //context.Result = new OkResult();
                    }
                    else
                    {
                        context.Result = new ForbidResult();
                    }
                }
                catch (Exception)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}

