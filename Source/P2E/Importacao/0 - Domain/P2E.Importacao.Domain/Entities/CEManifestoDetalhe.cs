using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_MANIFESTO_DETALHE" )]
    public class CEManifestoDetalhe : CustomNotifiable
    {
        public CEManifestoDetalhe()
        {

        }

        public CEManifestoDetalhe( string tx_ce_mercante,
                           string tx_tipo_manifesto,
                           string tx_porto_carrega,
                           string tx_porto_descarrega)
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_TIPO_MANIFESTO = tx_tipo_manifesto;
            TX_PORTO_CARREGA = tx_porto_carrega;
            TX_PORTO_DESCARREGA = tx_porto_descarrega;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_MAN_DET { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_TIPO_MANIFESTO { get; set; }
        public string TX_PORTO_CARREGA { get; set; }
        public string TX_PORTO_DESCARREGA { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
