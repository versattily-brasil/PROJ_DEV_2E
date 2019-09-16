using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    public class AgendaBot
    {
        public int CD_AGENDA_BOT { get; set; }
        public int CD_AGENDA { get; set; }
        public int CD_BOT { get; set; }
        public int NR_ORDEM_EXEC { get; set; }
        public string TX_PARAM_EXEC { get; set; }

        public Agenda Agenda { get; set; }
        public Bot Bot { get; set; }

        public String NomeBot { get { return Bot?.TX_NOME; }}
        public String DescBot { get { return Bot?.TX_DESCRICAO; } }

        public eStatusExec Status { get; set; }

    }
}
