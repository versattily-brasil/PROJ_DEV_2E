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

        public string Nome { get;  set; }
        public string Descricao { get; set; }
        public string Servico { get; set; }
        public string Url { get; set; }

        public DataPage<P2E.SSO.Domain.Entities.Rotina> DataPage { get; set; }
    }
}
