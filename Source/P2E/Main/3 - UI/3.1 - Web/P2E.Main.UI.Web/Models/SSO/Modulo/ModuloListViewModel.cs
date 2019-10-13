using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.SSO.Modulo
{
    public class ModuloListViewModel
    {
        public ModuloListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Modulo>();
        }
        public string TX_DSC { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.Modulo> DataPage { get; set; }
    }
}
