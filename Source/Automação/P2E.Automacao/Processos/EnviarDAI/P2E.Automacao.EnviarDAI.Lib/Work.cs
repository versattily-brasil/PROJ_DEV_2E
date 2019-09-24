using P2E.Automacao.Entidades;
using P2E.Automacao.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnviarDAI.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"http://online.sefaz.am.gov.br/diselada/consultadi.asp";

        private string _urlApiBase;
        private List<Importacao> registros;
        #endregion
    }
}
