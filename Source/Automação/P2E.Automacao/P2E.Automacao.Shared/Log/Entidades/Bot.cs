using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Shared.Log.Entidades
{
    [Table("TB_BOT")]
    public class Bot 
    {
        [Key]
        [Identity]
        public int CD_BOT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DESCRICAO { get; set; }
        public int OP_ATIVO { get; set; }
    }
}
