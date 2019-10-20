using P2E.Shared.Model;

namespace P2E.Main.UI.Web.Models.IMP.Moeda
{
    public class MoedaListViewModel
    {
        public MoedaListViewModel()
        {
            DataPage = new DataPage<P2E.Importacao.Domain.Entities.Moeda>();
        }

        public string codigoImportacao { get; set; }
        public string descricao { get; set; }
        public string codigoExportacao { get; set; }

        public DataPage<P2E.Importacao.Domain.Entities.Moeda> DataPage { get; set; }
    }
}
