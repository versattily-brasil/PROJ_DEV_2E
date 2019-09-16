using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    public class Agenda
    {
        public int CD_AGENDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public eStatusGenerico OP_ATIVO { get; set; }
        public eFormaExec OP_FORMA_EXEC { get; set; }
        public eStatusGenerico OP_REPETE { get; set; }
        public eTipoRepete OP_TIPO_REP { get; set; }
        public DateTime DT_DATA_EXEC_PLAN { get; set; }
        public TimeSpan HR_HORA_EXEC_PLAN { get; set; }
        public DateTime? DT_DATA_ULTIMA_EXEC { get; set; }
        public eStatusExec OP_ULTIMO_STATUS { get; set; }
        public eStatusExec OP_STATUS { get; set; }

        public IEnumerable<AgendaBot> Bots { get; set; }
    }
}
