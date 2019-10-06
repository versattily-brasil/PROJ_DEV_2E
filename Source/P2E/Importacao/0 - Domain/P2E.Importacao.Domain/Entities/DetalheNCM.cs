using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_DETALHE_NCM")]
    public class DetalheNCM : CustomNotifiable
    {
        public DetalheNCM()
        {

        }

        public DetalheNCM(string tx_sfncm_codigo,
                         string tx_sfncm_detalhe,
                         string tx_sfncm_descricao)
        {
            TX_SFNCM_CODIGO = tx_sfncm_codigo?.Trim();
            TX_SFNCM_DETALHE = tx_sfncm_detalhe?.Trim();
            TX_SFNCM_DESCRICAO = tx_sfncm_descricao?.Trim();

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_DET_NCM { get; set; }
        public string TX_SFNCM_CODIGO { get; set; }
        public string TX_SFNCM_DETALHE { get; set; }
        public string TX_SFNCM_DESCRICAO { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_SFNCM_DESCRICAO))
                AddNotification("TX_SFNCM_DESCRICAO", $"Descrição é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{TX_SFNCM_DESCRICAO.ToString()}";
    }
}
