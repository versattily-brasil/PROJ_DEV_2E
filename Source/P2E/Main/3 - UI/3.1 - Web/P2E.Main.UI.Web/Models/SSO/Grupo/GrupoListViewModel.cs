using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.SSO.Grupo
{
    public class GrupoListViewModel
    {
        public GrupoListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Grupo>();
        }
        public string descricao { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.Grupo> DataPage { get; set; }
    }
}
