using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using P2E.Shared.ValuesObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_PAR_NEG")]
    [Serializable]
    public class ParceiroNegocio : CustomNotifiable
    {
        public ParceiroNegocio()
        {
            //IsValid();
        }

        //public ParceiroNegocio(string tXT_RZSOC = "", Document cNPJ = null)
        public ParceiroNegocio(string tXT_RZSOC = "", string cNPJ = null)
        {
            TXT_RZSOC = tXT_RZSOC;
            CNPJ = cNPJ;

            IsValid();
        }

        [Key]
        [Identity]
        public long CD_PAR { get; set; }

        public string TXT_RZSOC { get; set; }
        //public Document CNPJ { get; set; }
        public string CNPJ { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TXT_RZSOC))
                AddNotification("TXT_RZSOC", $"Razão social é obrigatório.");

            if (string.IsNullOrEmpty(CNPJ))
                AddNotification("CNPJ", $"CNPJ é obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TXT_RZSOC.ToString()}";
    }
}
