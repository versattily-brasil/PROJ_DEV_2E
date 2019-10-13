using P2E.SSO.Domain.Entities;
using System.Collections.Generic;

namespace P2E.SSO.API.ViewModel
{
    public class ModuloVM
    {
        public IEnumerable<Modulo> Lista { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string tx_dsc { get; set; }
    }
}
