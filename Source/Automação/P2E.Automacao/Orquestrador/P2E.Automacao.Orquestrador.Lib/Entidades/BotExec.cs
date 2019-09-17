using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    public class BotExec
    {
        public int CD_BOT_EXEC { get; set; }
        public int CD_BOT { get; set; }
        public int CD_AGENDA_EXEC { get; set; }
        public eStatusExec OP_STATUS_BOT_EXEC { get; set; }
        public int NR_ORDEM_EXEC { get; set; }

        public AgendaExec AgendaExec { get; set; }
        public Bot Bot { get; set; }

        public DateTime? DT_INICIO_EXEC { get; set; }
        public DateTime? DT_FIM_EXEC { get; set; }
    }
}
