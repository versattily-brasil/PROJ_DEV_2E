using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_MOEDA")]
    public class Moeda : CustomNotifiable
    {
        public Moeda()
        {

        }

        public Moeda(string tx_cod_imp,string tx_descricao, string tx_cod_exp)
        {
            TX_COD_IMP = tx_cod_imp;
            TX_DESCRICAO = tx_descricao;
            TX_COD_EXP = tx_cod_exp;

            //IsValid();
        }


        [Key]
        [Identity]
        public int CD_MOEDA { get; set; }
        public string TX_COD_IMP { get; set; }
        public string TX_DESCRICAO { get; set; }
        public string TX_COD_EXP { get; set; }

        //public bool IsValid()
        //{
        //    if (string.IsNullOrEmpty(TX_DESCRICAO))
        //        AddNotification("TX_DESCRICAO", $"Descrição é um campo obrigatório.");

        //    return Valid;
        //}

        public override string ToString() => $"{TX_DESCRICAO.ToString()}";
    }
}
