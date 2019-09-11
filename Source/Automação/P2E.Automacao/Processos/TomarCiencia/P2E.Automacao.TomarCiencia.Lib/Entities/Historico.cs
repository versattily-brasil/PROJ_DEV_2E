using System;

namespace P2E.Automacao.TomarCiencia.Lib.Entities
{
    public class Historico
    {
        public int CD_HIST { get; set; }
        public int CD_IMP { get; set; }
        public int CD_IMP_STATUS { get; set; }
        public int CD_IMP_CANAL { get; set; }
        public string TX_NUM_DEC { get; set; }
        public DateTime DT_DATA { get; set; }
        public DateTime HR_HORA { get; set; }
    }
}
