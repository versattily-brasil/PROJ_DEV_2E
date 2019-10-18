using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_HIST")]
    public class Historico : CustomNotifiable
    {
        public Historico()
        {

        }

        public Historico(int cd_imp,
                         int cd_imp_status,
                         int cd_imp_canal,
                         string tx_num_dec,
                         DateTime dt_data,
                         DateTime hr_hora)
        {
            CD_IMP = cd_imp;
            CD_IMP_STATUS = cd_imp_status;
            CD_IMP_CANAL = cd_imp_canal;
            TX_NUM_DEC = tx_num_dec;
            DT_DATA = dt_data;
            HR_HORA = hr_hora;

        }

        [Key]
        [Identity]
        public int CD_HIST { get; set; }
        public int CD_IMP { get; set; }
        public int CD_IMP_STATUS { get; set; }
        public int CD_IMP_CANAL { get; set; }
        public string TX_NUM_DEC { get; set; }
        public DateTime? DT_DATA { get; set; }
        public DateTime? HR_HORA { get; set; }

    }
}
