using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P2E.SSO.Domain.Entities;
namespace P2E.Main.UI.Web.Models.SSO.ParceiroNegocio
{
    public class ParceiroNegocioViewModel
    {
        public long CD_PAR { get; set; }
        public string TXT_RZSOC { get; set; }
        public string CNPJ { get; set; }

        public List<P2E.SSO.Domain.Entities.Modulo> Modulos { get; set; }
        public List<P2E.SSO.Domain.Entities.Servico> Servicos { get; set; }
    }
}
