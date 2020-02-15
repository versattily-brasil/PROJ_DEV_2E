using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_COMP_FRETE_LONGO_CURSO" )]
    public class CECompFreteLongoCurso : CustomNotifiable
    {
        public CECompFreteLongoCurso()
        {

        }

        public CECompFreteLongoCurso( int cd_dados_long_imp,
                           string tx_tipo_componente,
                           decimal vl_valor,
                           string tx_moeda,
                            string tx_recolhimento )
        {
            CD_DADOS_LONG_IMP = cd_dados_long_imp;
            TX_TIPO_COMPONENTE = tx_tipo_componente;
            VL_VALOR = vl_valor;
            TX_MOEDA = tx_moeda;
            TX_RECOLHIMENTO = tx_recolhimento;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_COMP_FRETE { get; set; }
        public int CD_DADOS_LONG_IMP { get; set; }
        public string TX_TIPO_COMPONENTE { get; set; }
        public decimal VL_VALOR { get; set; }
        public string TX_MOEDA { get; set; }
        public string TX_RECOLHIMENTO { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_MOEDA ) )
                AddNotification( "TX_MOEDA", $"Valor é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_MOEDA.ToString()}";
    }
}
