using P2E.Importacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Importacao.API.ViewModel
{
    public class TriagemBotVM
    {
        public IEnumerable<TriagemBot> Lista { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string nr_di { get; set; }
    }
}
