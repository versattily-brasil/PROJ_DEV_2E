using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Automacao.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Automacao.Shared.Log.Entidades
{
    [Table("TB_AGENDA_EXEC")]
    public class AgendaExec
    {
        [Key]
        [Identity]
        public int CD_AGENDA_EXEC { get; set; }
        public int CD_AGENDA { get; set; }
        public DateTime? DT_INICIO_EXEC { get; set; }
        public DateTime? DT_FIM_EXEC { get; set; }
        public eStatusExec OP_STATUS_AGENDA_EXEC { get; set; }
    }
}
