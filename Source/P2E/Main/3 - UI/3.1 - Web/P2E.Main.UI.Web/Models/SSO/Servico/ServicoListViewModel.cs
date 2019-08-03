using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Models.SSO.Servico
{
    public class ServicoListViewModel
    {
        public ServicoListViewModel()
        {
            DataPage = new DataPage<P2E.SSO.Domain.Entities.Servico>();
        }
        public string TXT_DEC { get; set; }
        public DataPage<P2E.SSO.Domain.Entities.Servico> DataPage { get; set; }
    }
}
