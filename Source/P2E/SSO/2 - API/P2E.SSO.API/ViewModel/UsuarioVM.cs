using P2E.Shared.Enum;
using P2E.SSO.Domain.Entities;
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
        public IEnumerable<Usuario> Lista { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }

        public int CD_USR { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_NOME { get; set; }
        public string TX_SENHA { get; set; }
        public eStatusUsuario OP_STATUS { get; set; }
    }
}
