using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnviarPLI.Lib.Entidades
{
    public class EnvioPLIDTO
    {
        public EnvioPLIDTO()
        {

        }

        public int CD_ENV_PLI { get; set; }
        public DateTime DT_DATA_REG { get; set; }
        public int NR_IMPORTADOR { get; set; }
        public eStatus OP_STATUS { get; set; }
        public DateTime DT_DATA_EXEC { get; set; }
        public string TX_LOG { get; set; }
        public string NR_CODIGO_ENVIO { get; set; }
    }
}
