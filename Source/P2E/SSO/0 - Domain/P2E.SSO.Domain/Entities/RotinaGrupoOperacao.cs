using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_ROT_GRP")]
    public class RotinaGrupoOperacao
    {
        [Key]
        [Identity]
        public int CD_ROT_GRP { get; set; }
        public int CD_ROT { get; set; }
        public int CD_GRP { get; set; }
        public int CD_OPR { get; set; }

        public Grupo Grupo { get; set; }
        public Operacao Operacao { get; set; }

        public Rotina Rotina { get; set; }

    }
}
