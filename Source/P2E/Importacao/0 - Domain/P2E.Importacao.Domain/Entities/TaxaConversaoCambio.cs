using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_TAXA_CONV_CAMBIO")]
    public class TaxaConversaoCambio : CustomNotifiable
    {
        public TaxaConversaoCambio()
        {

        }

        public TaxaConversaoCambio(string tx_moeda,
                                   string tx_descricao, 
                                   DateTime dt_inicio_vigencia, 
                                   DateTime dt_fim_vigencia, 
                                   decimal vl_taxa_conversao)
        {
            TX_MOEDA = tx_moeda;
            TX_DESCRICAO = tx_descricao;
            DT_INICIO_VIGENCIA = dt_inicio_vigencia;
            DT_FIM_VIGENCIA = dt_fim_vigencia;
            VL_TAXA_CONVERSAO = vl_taxa_conversao;
        }

        [Key]
        [Identity]
        public int CD_TAXA_CAMBIO { get; set; }
        public string TX_MOEDA { get; set; }
        public string TX_DESCRICAO { get; set; }
        public DateTime DT_INICIO_VIGENCIA { get; set; }
        public DateTime DT_FIM_VIGENCIA { get; set; }
        public decimal VL_TAXA_CONVERSAO { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_DESCRICAO))
                AddNotification("TX_DESCRICAO", $"Descrição é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{TX_DESCRICAO.ToString()}";
    }
}
