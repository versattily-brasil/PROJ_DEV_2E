using P2E.Importacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Importacao.API.ViewModel
{
    public class CEMercanteVM
    {
        public IEnumerable<CEMercante> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_ce_mercante { get; set; }
    }
}
