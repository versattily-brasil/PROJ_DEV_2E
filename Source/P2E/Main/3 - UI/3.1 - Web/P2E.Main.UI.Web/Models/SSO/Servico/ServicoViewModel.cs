using P2E.Main.UI.Web.Models.SSO.Rotina;
using System;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.SSO.Servico
{
    [Serializable]
    public class ServicoViewModel
    {
        public long CD_SRV { get; set; }
        public string TXT_DEC { get; set; }

        public List<RotinaViewModel> RotinasViewModel { get; set; }
    }
}
