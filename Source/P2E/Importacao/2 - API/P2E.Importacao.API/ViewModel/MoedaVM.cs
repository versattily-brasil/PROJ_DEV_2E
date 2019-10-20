using P2E.Importacao.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Importacao.API.ViewModel
{
    public class MoedaVM
    {
        public IEnumerable<Moeda> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_cod_imp { get; set; }
        public string tx_descricao { get; set; }
        public string tx_cod_exp { get; set; }
    }
}
