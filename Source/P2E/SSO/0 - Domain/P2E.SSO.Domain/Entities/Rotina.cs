using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_ROT")]
    public class Rotina : Notifiable
    {
        public Rotina() {}

        public Rotina(string tx_nome, string tx_dsc, int op_tipo)
        {
            TX_NOME = tx_nome;
            TX_DSC = tx_dsc;
            OP_TIPO = op_tipo;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_ROT { get; private set; }
        public string TX_NOME { get; private set; }
        public string TX_DSC { get; private set; }
        public int OP_TIPO { get; private set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_NOME.Trim()))
                AddNotification("TX_NOME", $"O Nome da Rotina é um campo obrigatório.");

            if (string.IsNullOrEmpty(TX_DSC.Trim()))
                AddNotification("TX_DSC", $"A Descrição da Rotina é um campo obrigatório.");

            if (OP_TIPO == 0)
                AddNotification("OP_TIPO", $"O Tipo da Rotina é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_NOME.ToString()}";
    }
}
