using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace P2E.SSO.Domain.Entities
{
    public class TB_MOD : Notifiable
    {
        public TB_MOD()
        {

        }

        public TB_MOD(string descricao)
        {
            TX_DSC = descricao;
        }

        [Key]
        [Identity]
        public int CD_MOD { get; private set; }
        public string TX_DSC { get; private set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_DSC.Trim()))
                AddNotification("Descrição", $"Descrição é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_DSC.ToString()}";
    }
}
