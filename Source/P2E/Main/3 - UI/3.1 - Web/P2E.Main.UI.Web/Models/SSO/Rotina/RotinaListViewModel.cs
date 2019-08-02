using P2E.Shared.Model;
using P2E.Shared.Enum;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    public class RotinaListViewModel
    {
        public RotinaListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Rotina>();
        }
        public int CD_ROT { get; private set; }
        public string TX_NOME { get; private set; }
        public string TX_DSC { get; private set; }
        public eTipoRotina OP_TIPO { get; private set; }
        public DataPage<P2E.SSO.Domain.Entities.Rotina> DataPage { get; set; }
    }
}
