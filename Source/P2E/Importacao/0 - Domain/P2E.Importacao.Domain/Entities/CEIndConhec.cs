using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_IND_CONHEC" )]
    public class CEIndConhec : CustomNotifiable
    {
        public CEIndConhec()
        {

        }

        public CEIndConhec( string tx_ce_mercante,
                           string tx_pend_afrmm,
                           string tx_bloq_conhec,
                           string tx_retificacao,
                           string tx_pend_tran_mar,
                           string tx_endos_conhec,
                           string tx_revi_afrmm )
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_PEND_AFRMM = tx_pend_afrmm;
            TX_BLOQ_CONHEC = tx_bloq_conhec;
            TX_RETIFICACAO = tx_retificacao;
            TX_PEND_TRAN_MAR = tx_pend_tran_mar;
            TX_ENDOS_CONHEC = tx_endos_conhec;
            TX_REVI_AFRMM = tx_revi_afrmm;


            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE_IND_CONHEC { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_PEND_AFRMM { get; set; }
        public string TX_BLOQ_CONHEC { get; set; }
        public string TX_RETIFICACAO { get; set; }
        public string TX_PEND_TRAN_MAR { get; set; }
        public string TX_ENDOS_CONHEC { get; set; }
        public string TX_REVI_AFRMM { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
