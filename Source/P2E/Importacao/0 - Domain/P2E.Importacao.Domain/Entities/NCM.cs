using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_NCM")]
    public class NCM : CustomNotifiable
    {
        public NCM()
        {

        }

        public NCM(string tx_nro_ncm,
                         string tx_descricao)
        {
            TX_NRO_NCM = tx_nro_ncm?.Trim();
            TX_DESCRICAO = tx_descricao?.Trim();

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_NCM { get; set; }
        public string TX_NRO_NCM { get; set; }
        public string TX_DESCRICAO { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_DESCRICAO))
                AddNotification("TX_DESCRICAO", $"Descrição é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{TX_DESCRICAO.ToString()}";
    }
}
