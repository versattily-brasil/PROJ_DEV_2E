using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_USR_GRP")]
    public class UsuarioGrupo
    {
        [Key]
        [Identity]
        public int CD_USR_GRP { get; set; }
        public int CD_USR { get; set; }
        public int CD_GRP { get; set; }
    }
}
