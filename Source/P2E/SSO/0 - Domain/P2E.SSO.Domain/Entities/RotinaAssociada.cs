using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_ROT_ASS")]
    public class RotinaAssociada
    {
        [Key]
        [Identity]
        public long CD_ASS { get; set; }
        public long CD_ROT_PRINCIPAL { get; set; }
        public long CD_ROT_ASS { get; set; }

        [NotMapped]
        public string NomeRotinaAssociada { get; set; }
    }
}
