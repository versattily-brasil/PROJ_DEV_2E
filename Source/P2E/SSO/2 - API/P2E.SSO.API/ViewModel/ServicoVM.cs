using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.ViewModel
{
    public class ServicoVM
    {
        //public int CD_SRV { get; set; }
        //public string TXT_DEC { get; set; }

        public IEnumerable<Servico> Lista { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string TXT_DEC { get; set; }
    }
}
