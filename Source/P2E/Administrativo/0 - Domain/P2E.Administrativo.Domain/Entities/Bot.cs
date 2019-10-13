using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Administrativo.Domain.Entities
{
    [Table("TB_BOT")]
    public class Bot : CustomNotifiable
    {
        [Key]
        [Identity]
        public int CD_BOT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DESCRICAO { get; set; }
        public int OP_ATIVO { get; set; }
    }
}
