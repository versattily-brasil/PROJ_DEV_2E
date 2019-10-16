using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Shared.Enum;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    [Serializable]
    public class RotinaViewModel
    {
        public RotinaViewModel()
        {
            RotinasAssociadas = new List<RotinaAssociada>(); 
        }
        public int CD_ROT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DSC { get; set; }
        public eTipoRotina OP_TIPO { get; set; }
        public int CD_SRV { get; set; }
        public string TX_URL { get; set; }

        public List<P2E.SSO.Domain.Entities.Servico> Servicos { get; set; }
        public List<RotinaViewModel> Rotinas { get; set; }
        
        public List<P2E.SSO.Domain.Entities.RotinaAssociada> RotinasAssociadas { get; set; }

        public List<RotinaServico> RotinaServico { get; set; } = new List<RotinaServico>();

        //public List<ServicoViewModel> ServicosViewModels { get; set; } = new List<ServicoViewModel>();

        public P2E.SSO.Domain.Entities.Servico Servico { get; set; }

        public List<OperacaoViewModel> OperacoesViewModel { get; set; }
    }
}
