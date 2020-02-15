using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_MANIFESTO" )]
    public class CEManifesto : CustomNotifiable
    {
        public CEManifesto()
        {

        }

        public CEManifesto( string tx_ce_mercante,
                           string tx_nro_viagem,
                           string tx_porto_carrega,
                           string tx_tipo_trafego,
                           string tx_cod_embarque,
                           string tx_emp_navega,
                           string tx_age_navega,
                           DateTime dt_encerra,
                           DateTime dt_operacao,
                           string tx_porto_descar,
                           string tx_total_conhec)
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_NRO_VIAGEM = tx_nro_viagem;
            TX_PORTO_CARREGA = tx_porto_carrega;
            TX_TIPO_TRAFEGO = tx_tipo_trafego;
            TX_COD_EMBARQUE = tx_cod_embarque;
            TX_EMP_NAVEGA = tx_emp_navega;
            TX_AGE_NAVEGA = tx_age_navega;
            DT_ENCERRA = dt_encerra;
            DT_OPERACAO = dt_operacao;
            TX_PORTO_DESCAR = tx_porto_descar;
            TX_TOTAL_CONHEC = tx_total_conhec;


            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE_MAN { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_NRO_VIAGEM { get; set; }
        public string TX_PORTO_CARREGA { get; set; }
        public string TX_TIPO_TRAFEGO { get; set; }
        public string TX_COD_EMBARQUE { get; set; }
        public string TX_EMP_NAVEGA { get; set; }
        public string TX_AGE_NAVEGA { get; set; }
        public DateTime DT_ENCERRA { get; set; }
        public DateTime DT_OPERACAO { get; set; }
        public string TX_PORTO_DESCAR { get; set; }
        public string TX_TOTAL_CONHEC { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
