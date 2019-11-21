using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.ViewModel
{
    /// <summary>
    /// Classe de apresentação de Operação na View
    /// </summary>
    [Serializable]

    public class OperacaoViewModel
    {
        public int CD_OPR { get; set; }
        public string TX_DSC { get; set; }

        public bool Check { get; set; }
    }
}
