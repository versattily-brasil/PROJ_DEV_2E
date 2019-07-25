using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.ViewModel
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaVM
    {
        public int CD_ROT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DSC { get; set; }        
        public int OP_TIPO { get; set; }
    }
}
