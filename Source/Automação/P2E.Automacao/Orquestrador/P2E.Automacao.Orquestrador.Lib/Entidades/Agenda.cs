using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Automacao.Orquestrador.Lib.Util.Enum;
using P2E.Automacao.Orquestrador.Lib.Util.Extensions;
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
        [NotMapped]

        public string sAtivo { get { return OP_ATIVO == 1 ? "Sim" : "Não"; } }

        public eFormaExec OP_FORMA_EXEC { get; set; }
        public int OP_REPETE { get; set; }
        [NotMapped]
        public string sREPETE { get { return OP_REPETE == 1? "Sim" : "Não"; } }
        public eTipoRepete OP_TIPO_REP { get; set; }
        public eStatusExec OP_STATUS { get; set; }

        public DateTime? DT_DATA_EXEC_PROG { get; set; }
        public TimeSpan HR_HORA_EXEC_PROG { get; set; }
        
        public int? CD_ULTIMA_EXEC { get; set; }// { get { return AgendaProgramada?.CD_AGENDA_EXEC; } }
        
        public DateTime? DT_DATA_INICIO_ULTIMA_EXEC { get; set; }//{ get { return AgendaProgramada?.DT_FIM_EXEC; } }
        public DateTime? DT_DATA_FIM_ULTIMA_EXEC { get; set; }//{ get { return AgendaProgramada?.DT_FIM_EXEC; } }

        public IEnumerable<AgendaBot> Bots { get; set; }

        public AgendaExec AgendaProgramada { get; set; }

        public AgendaExec UltimaAgendaExecutada { get; set; }
        public override string ToString() => $"{TX_DESCRICAO?.ToString()}";

        [NotMapped]
        public string Status { get { return OP_STATUS.GetDescription(); } }

        [NotMapped]
        public string tipoRepeticao { get { return OP_TIPO_REP.GetDescription(); } }
    }
}
