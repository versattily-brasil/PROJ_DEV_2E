using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnviarPLI.Lib.Entidades.EstruturaPLI
{
    [Serializable]
    public class ItemMatriz
    {
        public int cdNcmProdFinal { get; set; }
        public int cdSuframa { get; set; }
        public int cdDestinacao { get; set; }
        public int cdUtilizacao { get; set; }
        public string cdTributacao { get; set; }
        public int numDecreto { get; set; }
        public int tpDocumentoConcessivo { get; set; }
        public int numDocumentoConcessivo { get; set; }
        public int dtInicioBeneficio { get; set; }
        public int dtFimBeneficio { get; set; }
    }
}
