using P2E.Shared.Enum;
using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.SSO.Usuario
{
    public class UsuarioListViewModel
    {
        public UsuarioListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Usuario>();
        }
        public int CD_USR { get; set; }
        public string TX_NOME { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_SENHA { get; set; }
        public eStatusUsuario OP_STATUS { get; set; }

        public DataPage<P2E.SSO.Domain.Entities.Usuario> DataPage { get; set; }
    }
}
