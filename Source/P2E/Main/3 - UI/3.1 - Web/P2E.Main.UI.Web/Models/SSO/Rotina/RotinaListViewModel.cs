using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    public class RotinaListViewModel
    {
        public RotinaListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Rotina>();
        }
        public string nome { get; set; }
        public string descricao { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.Rotina> DataPage { get; set; }
    }
}
