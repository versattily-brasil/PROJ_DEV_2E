using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.AcompanharDespachos.Lib.Entities
{
    public class Importacao
    {
        public class TBImportacao
        {
            public int CD_IMP { get; set; }
            public int NUM_PI { get; set; }
            public int CD_IMP_STATUS { get; set; }
            public int CD_IMP_CANAL { get; set; }
            public string TX_NUM_DEC { get; set; }
            public DateTime DT_DATA_DES { get; set; }
            public decimal VL_MULTA { get; set; }
            public string TX_NOME_FISCAL { get; set; }
            public DateTime DT_DATA_CANAL { get; set; }
            public DateTime DT_DATA_DISTR { get; set; }
            public string TX_DOSSIE { get; set; }
            public DateTime DT_DATA_DOSS { get; set; }
        }
    }
}
