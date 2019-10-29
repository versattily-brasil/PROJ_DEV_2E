using P2E.Importacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Importacao.API.ViewModel
{
    public class NCMVM
    {
        public IEnumerable<NCM> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_nro_ncm { get; set; }
        public string tx_descricao { get; set; }
    }
}
