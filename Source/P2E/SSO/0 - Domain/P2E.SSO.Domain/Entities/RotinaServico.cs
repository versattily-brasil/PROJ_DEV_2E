using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_ROT_SRV")]
    public class RotinaServico
    {
        [Key]
        [Identity]
        public int CD_ROT_SRV { get; set; }
        public int CD_ROT { get; set; }
        public int CD_SRV { get; set; }

        public Servico Servico { get; set; }
    }
}
