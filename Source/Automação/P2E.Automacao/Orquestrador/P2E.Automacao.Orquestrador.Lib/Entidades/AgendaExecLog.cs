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
    [Table("TB_AGENDA_EXEC_LOG")]
    public class AgendaExecLog
    {
        [Key]
        [Identity]
        public int CD_AGENDA_EXEC_LOG { get; set; }
        public eTipoLog OP_TIPO_LOG { get; set; }
        public string TX_MENSAGEM { get; set; }
        public int CD_AGENDA_EXEC { get; set; }
        public DateTime DT_DATAHORA_REG { get; set; }

    }
}
