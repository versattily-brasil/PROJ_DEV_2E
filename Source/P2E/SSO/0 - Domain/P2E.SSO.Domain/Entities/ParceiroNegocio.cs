using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using P2E.Shared.ValuesObject;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_PAR_NEG")]
    public class ParceiroNegocio : CustomNotifiable
    {
        public ParceiroNegocio()
        {
            //IsValid();
        }

        public ParceiroNegocio(string tXT_RZSOC, string cNPJ, string tx_email)
        {
            TXT_RZSOC = tXT_RZSOC?.Trim();
            CNPJ = cNPJ?.Trim();
            TX_EMAIL = tx_email;

            IsValid();
        }

        [Key]
        [Identity]
        public long CD_PAR { get; set; }

        public string TXT_RZSOC { get; set; }
        //public Document CNPJ { get; set; }
        public string CNPJ { get; set; }
        public string TX_EMAIL { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TXT_RZSOC))
                AddNotification("TXT_RZSOC", $"Razão social é obrigatório.");

            if (string.IsNullOrEmpty(CNPJ))
                AddNotification("CNPJ", $"CNPJ é obrigatório.");
            else
            {
                //if (!Funcoes.IsCnpj(CNPJ))
                //{
                //    AddNotification("CNPJ", $"CNPJ é inválido.");
                //}
            }
            return Valid;
        }

        public override string ToString() => $"{TXT_RZSOC.ToString()}";

        public List<ParceiroNegocioServicoModulo> ParceiroNegocioServicoModulo { get; set; } = new List<ParceiroNegocioServicoModulo>();

        public List<Modulo> Modulo { get; set; } = new List<Modulo>();
        public List<Servico> Servico { get; set; } = new List<Servico>();
    }
}
