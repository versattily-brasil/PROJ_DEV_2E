using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Enum;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Administrativo.Domain.Entities
{
    [Table("TB_AGENDA")]
    public class Agenda : CustomNotifiable
    {
        [Key]
        [Identity]
        public int CD_AGENDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public int OP_ATIVO { get; set; }
        public eFormaExec OP_FORMA_EXEC { get; set; }
        public int OP_REPETE { get; set; }

        [NotMapped]
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


        [NotMapped]
        public eStatusExec OP_ULTIMO_STATUS_EXEC { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_DESCRICAO))
                AddNotification("TX_DESCRICAO", $"Descrição é um campo obrigatório.");

            return Valid;
        }
    }
}
