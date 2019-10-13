using P2E.SSO.Domain.Entities;
using System.Collections.Generic;

namespace P2E.SSO.API.ViewModel
{
    public class ParceiroNegocioVM
    {
        public IEnumerable<ParceiroNegocio> Lista { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public string razaoSocial { get; set; }
        public string CNPJ { get; set; }
    }
}
