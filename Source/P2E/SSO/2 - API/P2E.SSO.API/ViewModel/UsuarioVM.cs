using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.ViewModel
{
    /// <summary>
    /// Classe de apresentação do Usuario na View
    /// </summary>
    public class UsuarioVM
    {
        public int CD_USR { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_NOME { get; set; }
        public string TX_SENHA { get; set; }
        public int OP_STATUS { get; set; }
    }
}
