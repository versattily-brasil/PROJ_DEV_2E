using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_MASTER" )]
    public class CEMaster : CustomNotifiable
    {
        public CEMaster()
        {

        }

        public CEMaster( string tx_nro_master,
                           string tx_situacao,
                           string tx_age_desconso,
                           string tx_emp,
                           int nr_qtd_filho_inf,
                           decimal nr_cubagem, 
                           string tx_incon_peso ,
                           string tx_incon_frete ,
                           int nr_qtd_filho_inc,
                           decimal nr_peso_bruto )
        {
            TX_NRO_MASTER = tx_nro_master;
            TX_SITUACAO = tx_situacao;
            TX_AGE_DESCONSO = tx_age_desconso;
            TX_EMP = tx_emp;
            NR_QTD_FILHO_INF = nr_qtd_filho_inf;
            NR_CUBAGEM = nr_cubagem;
            TX_INCON_PESO = tx_incon_peso;
            TX_INCON_FRETE = tx_incon_frete;
            NR_QTD_FILHO_INC = nr_qtd_filho_inc;
            NR_PESO_BRUTO = nr_peso_bruto;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE_MASTER { get; set; }
        public string TX_NRO_MASTER { get; set; }
        public string TX_SITUACAO { get; set; }
        public string TX_AGE_DESCONSO { get; set; }
        public string TX_EMP { get; set; }
        public int NR_QTD_FILHO_INF { get; set; }
        public decimal NR_CUBAGEM { get; set; }
        public string TX_INCON_PESO { get; set; }
        public string TX_INCON_FRETE { get; set; }
        public int NR_QTD_FILHO_INC { get; set; }
        public decimal NR_PESO_BRUTO { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_NRO_MASTER ) )
                AddNotification( "TX_NRO_MASTER", $"Master é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_NRO_MASTER.ToString()}";
    }
}
