using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.SSO.Domain.Entities
{
    [Table("TB_OPR")]
    public class Operacao : CustomNotifiable
    {
        public Operacao() {}

        public Operacao(string tx_dsc)
        {            
            TX_DSC = tx_dsc?.Trim();

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_OPR { get; set; }
        public string TX_DSC { get; set; }

        public bool IsValid()
        {            
            if (string.IsNullOrEmpty(TX_DSC))
                AddNotification("TX_DSC", $"A Descrição da Operação é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_DSC.ToString()}";
    }
}
