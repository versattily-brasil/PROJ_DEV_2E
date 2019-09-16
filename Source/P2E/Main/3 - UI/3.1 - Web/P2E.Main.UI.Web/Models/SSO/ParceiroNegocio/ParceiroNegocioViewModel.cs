using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.SSO.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.SSO.ParceiroNegocio
{
    public class ParceiroNegocioViewModel
    {
        public long CD_PAR { get; set; }
        public string TXT_RZSOC { get; set; }
        public string CNPJ { get; set; }
        public string TX_EMAIL { get; set; }

        public List<P2E.SSO.Domain.Entities.Modulo> Modulos { get; set; }
        public List<P2E.SSO.Domain.Entities.Servico> Servicos { get; set; }

        public List<ParceiroNegocioServicoModulo> ParceiroNegocioServicoModulo { get; set; } = new List<ParceiroNegocioServicoModulo>();

        public List<ModuloViewModel> Modulo { get; set; } = new List<ModuloViewModel>();
        public List<ServicoViewModel> Servico { get; set; } = new List<ServicoViewModel>();
    }
}
