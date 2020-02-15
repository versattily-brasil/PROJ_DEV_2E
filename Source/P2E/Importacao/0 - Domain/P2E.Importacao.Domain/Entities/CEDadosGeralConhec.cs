using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_DADOS_GERAL_CONHEC" )]
    public class CEDadosGeralConhec : CustomNotifiable
    {
        public CEDadosGeralConhec()
        {

        }

        public CEDadosGeralConhec( string tx_ce_mercante,
                                    string tx_tipo_conhec,
                                    DateTime dt_data_emissao,
                                    decimal nr_cubagem,
                                    string tx_porto_origem,
                                    string tx_cpf_cnpj_consig,
                                    string tx_id_embarcador,
                                    string tx_desc_mercadoria,
                                    string tx_observacao,
                                    string tx_categ_carga,
                                    string tx_conhec_emb,
                                    decimal nr_peso_bruto,
                                    string tx_porto_dest )
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_TIPO_CONHEC = tx_tipo_conhec;
            DT_DATA_EMISSAO = dt_data_emissao;
            NR_CUBAGEM = nr_cubagem;
            TX_PORTO_ORIGEM = tx_porto_origem;
            TX_CPF_CNPJ_CONSIG = tx_cpf_cnpj_consig;
            TX_ID_EMBARCADOR = tx_id_embarcador;
            TX_DESC_MERCADORIA = tx_desc_mercadoria;
            TX_OBSERVACAO = tx_observacao;
            TX_CATEG_CARGA = tx_categ_carga;
            TX_CONHEC_EMB = tx_conhec_emb;
            NR_PESO_BRUTO = nr_peso_bruto;
            TX_PORTO_DEST = tx_porto_dest;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_CE_CONHEC { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_TIPO_CONHEC { get; set; }
        public DateTime DT_DATA_EMISSAO { get; set; }
        public decimal NR_CUBAGEM { get; set; }
        public string TX_PORTO_ORIGEM { get; set; }
        public string TX_CPF_CNPJ_CONSIG { get; set; }
        public string TX_ID_EMBARCADOR { get; set; }
        public string TX_DESC_MERCADORIA { get; set; }
        public string TX_OBSERVACAO { get; set; }
        public string TX_CATEG_CARGA { get; set; }
        public string TX_CONHEC_EMB { get; set; }
        public decimal NR_PESO_BRUTO { get; set; }
        public string TX_PORTO_DEST { get; set; }


        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
