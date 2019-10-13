using P2E.Importacao.Domain.Entities;
using System;
using System.Collections.Generic;

namespace P2E.Importacao.API.ViewModel
{
    public class TaxaConversaoCambioVM
    {
        public IEnumerable<TaxaConversaoCambio> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_moeda { get; set; }
        public string tx_descricao { get; set; }
        public DateTime dt_inicial_vigencia { get; set; }
        public DateTime dt_fim_vigencia { get; set; }
        public decimal vl_taxa_conversao { get; set; }
    }
}
