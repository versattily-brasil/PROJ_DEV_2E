using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Entidades
{
    public class TaxaCambio
    {
        public int CD_TAXA_CAMBIO { get; set; }
        public string TX_MOEDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public DateTime DT_INICIO_VIGENCIA { get; set; }
        public DateTime DT_FIM_VIGENCIA { get; set; }
        public decimal VL_TAXA_CONVERSAO { get; set; }
    }
}
