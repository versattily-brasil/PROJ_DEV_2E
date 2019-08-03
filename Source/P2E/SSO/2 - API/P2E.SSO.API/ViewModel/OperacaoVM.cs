using P2E.Shared.Enum;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;

namespace P2E.SSO.API.ViewModel
{
    /// <summary>
    /// Classe de apresentação de Operação na View
    /// </summary>
    public class OperacaoVM
    {
        public IEnumerable<Operacao> Lista { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }


        public int CD_OPR { get; set; }        
        public string TX_DSC { get; set; }
    }
}
