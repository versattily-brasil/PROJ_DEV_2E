using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    [Table("TB_BOT_EXEC_LOG")]
    public class BotExecLog 
    {
        [Key]
        [Identity]
        public int CD_BOT_EXEC_LOG { get; set; }
        public string OP_TIPO_LOG { get; set; }
        public string TX_MENSAGEM { get; set; }
        public string TX_INF_ADICIONAL { get; set; }
        public int CD_BOT_EXEC { get; set; }
        public DateTime DT_DATAHORA_REG { get; set; }

        [NotMapped]
        public string Log { get { return $"[{OP_TIPO_LOG}] - [{DT_DATAHORA_REG}] - {TX_MENSAGEM}"; } }
    }
}
