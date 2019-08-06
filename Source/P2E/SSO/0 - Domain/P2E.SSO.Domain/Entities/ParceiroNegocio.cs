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
    public class ParceiroNegocio : CustomNotifiable
    {
        public ParceiroNegocio()
        {
            //IsValid();
        }
        
        public ParceiroNegocio(string tXT_RZSOC, string cNPJ)
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
            else
            {
                if (!IsCnpj(CNPJ))
                {
                    AddNotification("CNPJ", $"CNPJ é inválido.");
                }
            }
            return Valid;
        }

        public override string ToString() => $"{TXT_RZSOC.ToString()}";

        public List<ParceiroNegocioServicoModulo> ParceiroNegocioServicoModulo { get; set; } = new List<ParceiroNegocioServicoModulo>();

        public List<Modulo> Modulo { get; set; } = new List<Modulo>();
        public List<Servico> Servico { get; set; } = new List<Servico>();

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
