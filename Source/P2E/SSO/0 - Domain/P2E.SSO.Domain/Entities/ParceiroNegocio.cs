using FluentValidator;
using P2E.Shared.ValuesObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_PAR_NEG")]
    public class ParceiroNegocio : Notifiable
    {
        public ParceiroNegocio()
        {
            //IsValid();
        }

        public ParceiroNegocio(string tXT_RZSOC = "", Document cNPJ = null)
        {
            TXT_RZSOC = tXT_RZSOC;
            CNPJ = cNPJ;

            IsValid();
        }

        public long CD_PAR { get; set; }
        public string TXT_RZSOC { get; set; }
        public Document CNPJ { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TXT_RZSOC))
                AddNotification("TXT_RZSOC", $"Razão social é obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TXT_RZSOC.ToString()}";
    }
}
