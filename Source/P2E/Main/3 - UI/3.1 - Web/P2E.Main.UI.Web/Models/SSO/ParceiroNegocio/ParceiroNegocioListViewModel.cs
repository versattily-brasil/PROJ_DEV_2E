using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.Models.SSO.ParceiroNegocio
{
    public class ParceiroNegocioListViewModel
    {
        public ParceiroNegocioListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.ParceiroNegocio>();
        }
        public string cnpj { get; set; }
        public string razaosocial { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.ParceiroNegocio> DataPage { get; set; }
    }
}
