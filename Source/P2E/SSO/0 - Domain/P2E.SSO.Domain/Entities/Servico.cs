using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_SRV")]
    public class Servico : CustomNotifiable
    {
        public Servico()
        {

        }

        public Servico(string txt_dec)
        {
            TXT_DEC = txt_dec?.Trim();

            IsValid();
        }

        [Key]
        [Identity]
        public long CD_SRV { get; set; }
        public string TXT_DEC { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TXT_DEC))
                AddNotification("TXT_DEC", $"Descrição é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{TXT_DEC.ToString()}";

        public List<Rotina> Rotinas { get; set; } = new List<Rotina>();
    }
}
