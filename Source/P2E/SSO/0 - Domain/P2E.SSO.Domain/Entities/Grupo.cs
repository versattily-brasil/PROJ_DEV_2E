using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace P2E.SSO.Domain.Entities
{
    [Table("TB_GRP")]
    public class Grupo : CustomNotifiable
    {
        public Grupo()
        {

        }

        public Grupo(string tx_dsc)
        {
            TX_DSC = tx_dsc;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_GRP { get; set; }
        public string TX_DSC { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_DSC))
                AddNotification("TX_DSC", $"Descrição é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{TX_DSC.ToString()}";
    }
}
