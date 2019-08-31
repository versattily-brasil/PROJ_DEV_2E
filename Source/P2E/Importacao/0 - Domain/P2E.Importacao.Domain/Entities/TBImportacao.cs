using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_IMP")]
    public class TBImportacao : CustomNotifiable
    {
        public TBImportacao()
        {

        }

        public TBImportacao(int num_pi,
                          int nr_num_dec, 
                          string tx_status,
                          string tx_canal,
                          DateTime dt_data_des,
                          decimal vl_multa,
                          string tx_nome_fiscal,
                          DateTime dt_data_canal,
                          DateTime dt_data_distr)
        {

            NUM_PI = num_pi;
            NR_NUM_DEC = nr_num_dec;
            TX_STATUS = tx_status;
            TX_CANAL = tx_canal;
            DT_DATA_DES = dt_data_des;
            VL_MULTA = vl_multa;
            TX_NOME_FISCAL = tx_nome_fiscal;
            DT_DATA_CANAL = dt_data_canal;
            DT_DATA_DISTR = dt_data_distr;
        }

        [Key]
        [Identity]
        public int CD_IMP { get; set; }
        public int NUM_PI { get; set; }
        public int NR_NUM_DEC { get; set; }
        public string TX_STATUS { get; set; }
        public string TX_CANAL { get; set; }
        public DateTime DT_DATA_DES { get; set; }
        public decimal VL_MULTA { get; set; }
        public string TX_NOME_FISCAL { get; set; }
        public DateTime DT_DATA_CANAL { get; set; }
        public DateTime DT_DATA_DISTR { get; set; }
    }
}
