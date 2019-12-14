using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_MERCANTE" )]
    public class CEMercante : CustomNotifiable
    {
        public CEMercante()
        {

        }

        public CEMercante( string tx_ce_mercante,
                           string tx_num_bl,
                           string tx_tipo,
                           string tx_num_manifesto,
                           DateTime dt_data_operacao)
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_NUM_BL = tx_num_bl;
            TX_NUM_BL = tx_tipo;
            TX_TIPO = tx_ce_mercante;
            TX_NUM_MANIFESTO = tx_num_manifesto;
            DT_DATA_OPERACAO = dt_data_operacao;


            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_NUM_BL { get; set; }
        public string TX_TIPO { get; set; }
        public string TX_NUM_MANIFESTO { get; set; }
        public DateTime DT_DATA_OPERACAO { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
