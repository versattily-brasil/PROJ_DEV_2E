using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_USR")]
    public class Usuario : Notifiable
    {
        public Usuario()
        {

        }

        public Usuario(int cd_usr, string tx_login, string tx_nome, string tx_senha, int op_status)
        {
            CD_USR = cd_usr;
            TX_LOGIN = tx_login;
            TX_NOME = tx_nome;
            TX_SENHA = tx_senha;
            OP_STATUS = op_status;
        }

        [Key]
        [Identity]
        public int CD_USR { get; set; }
        public string TX_NOME { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_SENHA { get; set; }
        public int OP_STATUS { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_LOGIN))
                AddNotification("TX_LOGIN", $"Login é um campo obrigatório.");
            if (string.IsNullOrEmpty(TX_LOGIN))
                AddNotification("TX_NOME", $"Nome é um campo obrigatório.");
            if (string.IsNullOrEmpty(TX_SENHA))
                AddNotification("TX_SENHA", $"Senha é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_LOGIN.ToString()}";

        public List<UsuarioModulo> UsuarioModulos { get; set; } = new List<UsuarioModulo>();
        public List<UsuarioGrupo> UsuarioGrupo { get; set; } = new List<UsuarioGrupo>();
    }
}
