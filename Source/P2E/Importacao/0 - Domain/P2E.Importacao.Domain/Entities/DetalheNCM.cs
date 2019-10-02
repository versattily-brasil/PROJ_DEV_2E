using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_DETALHE_NCM")]
    public class DetalheNCM : CustomNotifiable
    {
        public DetalheNCM()
        {

        }

        public DetalheNCM(int cd_det_ncm,
                         string tx_sfncm_codigo,
                         string tx_sfncm_detalhe,
                         string tx_sfncm_descricao)
        {
            CD_DET_NCM = cd_det_ncm;
            TX_SFNCM_CODIGO = tx_sfncm_codigo;
            TX_SFNCM_DETALHE = tx_sfncm_detalhe;
            TX_SFNCM_DESCRICAO = tx_sfncm_descricao;
        }

        [Key]
        [Identity]
        public int CD_DET_NCM { get; set; }
        public string TX_SFNCM_CODIGO { get; set; }
        public string TX_SFNCM_DETALHE { get; set; }
        public string TX_SFNCM_DESCRICAO { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TX_SFNCM_DESCRICAO))
                AddNotification("TX_SFNCM_DESCRICAO", $"Descrição é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{TX_SFNCM_DESCRICAO.ToString()}";
    }
}
