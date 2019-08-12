using P2E.Shared.Enum;
using System.Collections.Generic;
using P2E.SSO.Domain.Entities;
using P2E.Main.UI.Web.Models.SSO.Grupo;
using P2E.Main.UI.Web.Models.SSO.Operacao;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaViewModel
    {
        public int CD_ROT { get; set; }        
        public string TX_NOME { get; set; }        
        public string TX_DSC { get; set; }
        public eTipoRotina OP_TIPO { get; set; }

        public List<P2E.SSO.Domain.Entities.Grupo> Grupos { get; set; }
        public List<P2E.SSO.Domain.Entities.Operacao> Operacoes { get; set; }

        public List<RotinaGrupoOperacao> RotinaGrupoOperacao { get; set; } = new List<RotinaGrupoOperacao>();

        public List<GrupoViewModel> Grupo { get; set; } = new List<GrupoViewModel>();
        public List<OperacaoViewModel> Operacao { get; set; } = new List<OperacaoViewModel>();
    }
}
