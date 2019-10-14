using P2E.Shared.Model;

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
        public string tx_email { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.ParceiroNegocio> DataPage { get; set; }
    }
}
