using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_USR_MOD")]
    public class UsuarioModulo : Notifiable
    {
        public UsuarioModulo()
        {

        }

        public UsuarioModulo(int cd_usr, int cd_mod)
        {
            CD_USR = cd_usr;
            CD_MOD = cd_mod;
        }

        [Key]
        [Identity]
        public int CD_USR_MOD { get; private set; }
        public int CD_USR { get; private set; }
        public int CD_MOD { get; private set; }
    }
}
