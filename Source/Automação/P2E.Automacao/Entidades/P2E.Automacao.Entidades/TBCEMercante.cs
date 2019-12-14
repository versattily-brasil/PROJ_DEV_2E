using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Entidades
{
    public class TBCEMercante
    {
        public int CD_CE { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_NUM_BL { get; set; }
        public string TX_TIPO { get; set; }
        public string TX_NUM_MANIFESTO { get; set; }
        public DateTime DT_DATA_OPERACAO { get; set; }
    }
}
