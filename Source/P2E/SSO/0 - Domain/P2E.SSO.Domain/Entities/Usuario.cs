using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Enum;
using P2E.Shared.Message;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_USR")]
    public class Usuario : CustomNotifiable
    {
        public Usuario()
        {

        }

        public Usuario(int cd_usr, string tx_login, string tx_nome, string tx_senha, eStatusUsuario op_status)
        {
            CD_USR = cd_usr;
            TX_LOGIN = tx_login?.Trim();
            TX_NOME = tx_nome?.Trim();
            TX_SENHA = tx_senha?.Trim();
            OP_STATUS = op_status;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_USR { get; set; }
        public string TX_NOME { get; set; }
        public string TX_LOGIN { get; set; }
        public string TX_SENHA { get; set; }
        public eStatusUsuario OP_STATUS { get; set; }

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

        public List<UsuarioModulo> UsuarioModulo { get; set; } = new List<UsuarioModulo>();
        public List<Modulo> Modulo { get; set; } = new List<Modulo>();
        public List<UsuarioGrupo> UsuarioGrupo { get; set; } = new List<UsuarioGrupo>();
        public List<Grupo> Grupo { get; set; } = new List<Grupo>();
        public List<RotinaUsuarioOperacao> RotinaUsuarioOperacao { get; set; } = new List<RotinaUsuarioOperacao>();

        [NotMapped]
        //[Compare("Password")]
        public string CONFIRMA_SENHA { get; set; }
    }
}
