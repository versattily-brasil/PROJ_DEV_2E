using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_IMP")]
    public class TBImportacao : CustomNotifiable
    {
        public TBImportacao()
        {

        }

        public TBImportacao(int num_pi,
                            int cd_imp_status,
                            int cd_imp_canal,
                            string tx_num_dec, 
                            DateTime dt_data_des,
                            decimal vl_multa,
                            string tx_nome_fiscal,
                            DateTime dt_data_canal,
                            DateTime dt_data_distr,
                            string tx_dossie,
                            DateTime dt_data_doss,
                            int op_extrato_pdf,
                            int op_extrato_xml)
        {

            NUM_PI = num_pi;
            TX_NUM_DEC = tx_num_dec;
            CD_IMP_STATUS = cd_imp_status;
            CD_IMP_CANAL = cd_imp_canal;
            DT_DATA_DES = dt_data_des;
            VL_MULTA = vl_multa;
            TX_NOME_FISCAL = tx_nome_fiscal;
            DT_DATA_CANAL = dt_data_canal;
            DT_DATA_DISTR = dt_data_distr;
            TX_DOSSIE = tx_dossie;
            DT_DATA_DOSS = dt_data_doss;
            OP_EXTRATO_PDF = op_extrato_pdf;
            OP_EXTRATO_XML = op_extrato_xml;
        }

        [Key]
        [Identity]
        public int CD_IMP { get; set; }
        public int NUM_PI { get; set; }
        public int CD_IMP_STATUS { get; set; }
        public int CD_IMP_CANAL { get; set; }
        public string TX_NUM_DEC { get; set; }
        public DateTime? DT_DATA_DES { get; set; }
        public decimal VL_MULTA { get; set; }
        public string TX_NOME_FISCAL { get; set; }
        public DateTime? DT_DATA_CANAL { get; set; }
        public DateTime? DT_DATA_DISTR { get; set; }
        public string TX_DOSSIE { get; set; }
        public DateTime DT_DATA_DOSS { get; set; }
        public DateTime? DT_DATA_EXO_ICMS { get; set; }
        public string UF_DI { get; set; }
        public int OP_EXTRATO_PDF { get; set; }
        public int OP_EXTRATO_XML { get; set; }
    }
}
