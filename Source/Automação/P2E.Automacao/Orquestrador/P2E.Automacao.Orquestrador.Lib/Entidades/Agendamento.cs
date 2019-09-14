using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    public class Agendamento
    {
        public int CD_AGENDAMENTO { get; set; }
        public int CD_PROCESSO { get; set; }
        public eTipoExecucao OP_TIPO_EXEC { get; set; } // 0 = Automatico , 1 = Manual
        public Processo Processo { get; set; }

        public DateTime DT_EXEC_PLAN { get; set; }

        public DateTime DT_ULTIMA_EXEC { get; set; }

        public eStatusAgendamento OP_STATUS { get; set; }
    }
}
