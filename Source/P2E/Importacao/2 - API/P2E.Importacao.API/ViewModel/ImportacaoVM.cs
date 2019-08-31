using P2E.Importacao.Domain.Entities;
using System.Collections.Generic;

namespace P2E.Importacao.API.ViewModel
{
    public class ImportacaoVM
    {
        public IEnumerable<TBImportacao> Lista { get; set; }
    }
}
