using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    [Table("TB_AGENDA")]
    public class Agenda
    {
        [Key]
        [Identity]
        public int CD_AGENDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public int OP_ATIVO { get; set; }
        public eFormaExec OP_FORMA_EXEC { get; set; }
        public int OP_REPETE { get; set; }
        public eTipoRepete OP_TIPO_REP { get; set; }
        public eStatusExec OP_STATUS { get; set; }

        public DateTime? DT_DATA_EXEC_PROG { get; set; }
        public TimeSpan HR_HORA_EXEC_PROG { get; set; }
        
        public int? CD_ULTIMA_EXEC { get { return UltimaAgendaExecutada?.CD_AGENDA_EXEC; } }
        public DateTime? DT_DATA_ULTIMA_EXEC { get { return UltimaAgendaExecutada?.DT_INICIO_EXEC; } }

        [NotMapped]
        public IEnumerable<AgendaBot> Bots { get; set; }

        [NotMapped]
        public AgendaExec AgendaProgramada { get; set; }

        [NotMapped]
        public AgendaExec UltimaAgendaExecutada { get; set; }
        public override string ToString() => $"{TX_DESCRICAO?.ToString()}";

    }
}
