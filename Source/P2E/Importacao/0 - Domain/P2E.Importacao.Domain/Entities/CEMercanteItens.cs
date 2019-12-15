using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_MERCANTE_ITENS" )]
    public class CEMercanteItens : CustomNotifiable
    {
        public CEMercanteItens()
        {

        }

        public CEMercanteItens( string tx_nro_ce,
                               string tx_codigo,
                               string tx_tipo,
                               decimal vl_peso,
                               string tx_pendente,
                               string tx_detalhamento)
        {
            TX_NRO_CE = tx_nro_ce;
            TX_CODIGO = tx_codigo;
            TX_TIPO = tx_tipo;
            VL_PESO = vl_peso;
            TX_PENDENTE = tx_pendente;
            TX_DETALHAMENTO = tx_detalhamento;



            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE_ITEM { get; set; }
        public string TX_NRO_CE { get; set; }
        public string TX_CODIGO { get; set; }
        public string TX_TIPO { get; set; }
        public decimal VL_PESO { get; set; }
        public string TX_PENDENTE { get; set; }
        public string TX_DETALHAMENTO { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_NRO_CE ) )
                AddNotification( "TX_NRO_CE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_NRO_CE.ToString()}";
    }
}
