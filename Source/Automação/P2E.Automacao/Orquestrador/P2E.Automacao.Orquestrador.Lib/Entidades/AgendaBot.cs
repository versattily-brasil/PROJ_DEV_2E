using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2E.Automacao.Orquestrador.Lib.Util.Extensions;
using P2E.Automacao.Shared.Enum;

namespace P2E.Automacao.Orquestrador.Lib.Entidades
{
    [Table("TB_AGENDA_BOT")]
    public class AgendaBot
    {
        [Key]
        [Identity]
        public int CD_AGENDA_BOT { get; set; }
        public int CD_AGENDA { get; set; }
        public int CD_BOT { get; set; }
        public int NR_ORDEM_EXEC { get; set; }
        public string TX_PARAM_EXEC { get; set; }

        public eStatusExec? CD_ULTIMO_STATUS_EXEC_BOT { get; set; }//{ get { return BotProgramado?.OP_STATUS_BOT_EXEC; } }
        public int? CD_ULTIMA_EXEC_BOT { get; set; }//{ get { return BotProgramado?.CD_BOT_EXEC; } }

        public Agenda Agenda { get; set; }
        public Bot Bot { get; set; }

        public BotExec UltimoBotExec { get; set; }
        public BotExec BotProgramado { get; set; }
        public BotExec BotExecutado { get; set; }

        [NotMapped]
        public string DescBot { get { return Bot?.TX_DESCRICAO; } }
        [NotMapped]
        public string NomeBot { get { return Bot?.TX_NOME; } }

        [NotMapped]
        public string Status { get { return CD_ULTIMO_STATUS_EXEC_BOT.GetDescription(); } }
    }
}
