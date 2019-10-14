using P2E.Main.UI.Web.Models.SSO.Grupo;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.Shared.Enum;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.SSO.Usuario
{
    public class UsuarioViewModel
    {
        public int CD_USR { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_NOME { get; set; }
        public string TX_SENHA { get; set; }
        public string CONFIRMA_SENHA { get; set; }
        public eStatusUsuario OP_STATUS { get; set; }

        public List<UsuarioModuloViewModel> UsuarioModulo { get; set; } = new List<UsuarioModuloViewModel>();
        public List<UsuarioGrupoViewModel> UsuarioGrupo { get; set; } = new List<UsuarioGrupoViewModel>();

        public List<ModuloViewModel> Modulo { get; set; } = new List<ModuloViewModel>();
        public List<GrupoViewModel> Grupo { get; set; } = new List<GrupoViewModel>();

        public IList<RotinaViewModel> Rotinas { get; set; } = new List<RotinaViewModel>();
        public List<OperacaoViewModel> Operacoes { get; set; } = new List<OperacaoViewModel>();
        public IList<ServicoViewModel> Servicos { get; set; } = new List<ServicoViewModel>();

        public List<RotinaUsuarioViewModel> RotinaUsuarioOperacao { get; set; } = new List<RotinaUsuarioViewModel>();
    }
}
