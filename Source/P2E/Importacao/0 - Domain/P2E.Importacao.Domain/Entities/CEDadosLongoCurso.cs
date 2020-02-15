using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table( "TB_CE_DADOS_LONGO_CURSO" )]
    public class CEDadosLongoCurso : CustomNotifiable
    {
        public CEDadosLongoCurso()
        {

        }

        public CEDadosLongoCurso( string tx_ce_mercante,
                           string tx_tipo_consignatario,
                           string tx_consignatario,
                           string tx_notify_part,
                           decimal vl_frete_total,
                           decimal vl_frete_basico,
                           string tx_pagamento,
                           string tx_moeda,
                           string tx_modalidade,
                           int cd_comp_frete,
                           DateTime dt_emissao_bl,
                           string tx_conhe_bl,
                           string tx_navio,
                           string tx_porto,
                           string tx_bl_servico,
                           string tx_terminal_descarrega,
                           string tx_pais_procede_carga,
                           string tx_uf_destino_carga,
                           string tx_cpf_usuario,
                           string tx_tipo_usuario,
                           DateTime dt_data,
                           string tx_nome,
                           string tx_endereco_ip,
                           string tx_hora )
        {
            TX_CE_MERCANTE = tx_ce_mercante;
            TX_TIPO_CONSIGNATARIO = tx_tipo_consignatario;
            TX_CONSIGNATARIO = tx_consignatario;
            TX_NOTIFY_PART = tx_notify_part;
            VL_FRETE_TOTAL = vl_frete_total;
            VL_FRETE_BASICO = vl_frete_basico;
            TX_PAGAMENTO = tx_pagamento;
            TX_MOEDA = tx_moeda;
            TX_MODALIDADE = tx_modalidade;
            CD_COMP_FRETE = cd_comp_frete;
            DT_EMISSAO_BL = dt_emissao_bl;
            TX_CONHE_BL = tx_conhe_bl;
            TX_NAVIO = tx_navio;
            TX_PORTO = tx_porto;
            TX_BL_SERVICO = tx_bl_servico;
            TX_TERMINAL_DESCARREGA = tx_terminal_descarrega;
            TX_PAIS_PROCEDE_CARGA = tx_pais_procede_carga;
            TX_UF_DESTINO_CARGA = tx_uf_destino_carga;
            TX_CPF_USUARIO = tx_cpf_usuario;
            TX_TIPO_USUARIO = tx_tipo_usuario;
            DT_DATA = dt_data;
            TX_NOME = tx_nome;
            TX_ENDERECO_IP = tx_endereco_ip;
            TX_HORA = tx_hora;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_DADOS_LONG_IMP { get; set; }
        public string TX_CE_MERCANTE { get; set; }
        public string TX_TIPO_CONSIGNATARIO { get; set; }
        public string TX_CONSIGNATARIO { get; set; }
        public string TX_NOTIFY_PART { get; set; }
        public decimal VL_FRETE_TOTAL { get; set; }
        public decimal VL_FRETE_BASICO { get; set; }
        public string TX_PAGAMENTO { get; set; }
        public string TX_MOEDA { get; set; }
        public string TX_MODALIDADE { get; set; }
        public int CD_COMP_FRETE { get; set; }
        public DateTime DT_EMISSAO_BL { get; set; }
        public string TX_CONHE_BL { get; set; }
        public string TX_NAVIO { get; set; }
        public string TX_PORTO { get; set; }
        public string TX_BL_SERVICO { get; set; }
        public string TX_TERMINAL_DESCARREGA { get; set; }
        public string TX_PAIS_PROCEDE_CARGA { get; set; }
        public string TX_UF_DESTINO_CARGA { get; set; }
        public string TX_CPF_USUARIO { get; set; }
        public string TX_TIPO_USUARIO { get; set; }
        public DateTime DT_DATA { get; set; }
        public string TX_NOME { get; set; }
        public string TX_ENDERECO_IP { get; set; }
        public string TX_HORA { get; set; }

        public bool IsValid()
        {
            if ( string.IsNullOrEmpty( TX_CE_MERCANTE ) )
                AddNotification( "TX_CE_MERCANTE", $"CE é um campo obrigatório." );
            return Valid;
        }

        public override string ToString() => $"{TX_CE_MERCANTE.ToString()}";
    }
}
