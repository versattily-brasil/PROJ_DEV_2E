using P2E.Administrativo.Domain.Entities;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.Shared.Enum;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.ADM.ProgramacaoBot
{
    public class AgendaViewModel
    {
        public int CD_AGENDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public int OP_ATIVO { get; set; }
        public eFormaExec OP_FORMA_EXEC { get; set; }
        public int OP_REPETE { get; set; }

        public string repete { get { return OP_REPETE == 1 ? "Sim" : "Não"; } }
        public eTipoRepete OP_TIPO_REP { get; set; }
        public eStatusExec OP_STATUS { get; set; }

        public DateTime? DT_DATA_EXEC_PROG { get; set; }
        public TimeSpan HR_HORA_EXEC_PROG { get; set; }

        public int? CD_ULTIMA_EXEC { get; set; }
        public DateTime? DT_DATA_ULTIMA_EXEC { get; set; }

        public IEnumerable<AgendaBot> Bots { get; set; }

        public AgendaExec AgendaProgramada { get; set; }

        public AgendaExec UltimaAgendaExecutada { get; set; }
        public override string ToString() => $"{TX_DESCRICAO?.ToString()}";


        public eStatusExec OP_ULTIMO_STATUS_EXEC { get; set; }
    }
}
