using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.SSO.Operacao
{
    public class OperacaoListViewModel
    {
        public OperacaoListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Operacao>();
        }
        public int CD_OPR { get; private set; }
        public string TX_DSC { get; private set; }
        public DataPage<P2E.SSO.Domain.Entities.Operacao> DataPage { get; set; }
    }
}
