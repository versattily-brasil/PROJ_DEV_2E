using P2E.Importacao.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Importacao.API.ViewModel
{
    public class DetalheNCMVM
    {
        public IEnumerable<DetalheNCM> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_sfncm_codigo { get; set; }
        public string tx_sfncm_detalhe { get; set; }
        public string tx_sfncm_descricao { get; set; }
    }
}
