using Newtonsoft.Json;
using P2E.Main.UI.Web.Models.SSO.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Extensions.Util
{
    public static class Permissoes
    {
        public static bool VerificarPermissao(string rotina, string operacao, ClaimsPrincipal principal)
        {
            try
            {
                var permissoesJson = principal.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata");
                var servicosViewModel = JsonConvert.DeserializeObject<List<ServicoViewModel>>(permissoesJson);

                if (servicosViewModel.Any(p => p.RotinasViewModel.Any(x => x.TX_NOME.Contains(rotina) && x.OperacoesViewModel.Any(o => o.TX_DSC.Contains(operacao)))))
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
    }
}
