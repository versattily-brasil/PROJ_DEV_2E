using P2E.Importacao.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Importacao.API.ViewModel
{
    public class HistoricoVM
    {
        public IEnumerable<Historico> Lista { get; set; }
    }
}
