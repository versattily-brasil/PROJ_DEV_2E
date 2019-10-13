using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_USR_MOD")]
    public class UsuarioModulo
    {
        [Key]
        [Identity]
        public int CD_USR_MOD { get; set; }
        public int CD_USR { get; set; }
        public int CD_MOD { get; set; }
    }
}
