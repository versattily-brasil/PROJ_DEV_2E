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

        public Historico(string tx_num_dec,
                          string tx_status,
                          string tx_canal,
                          DateTime dt_data,
                          DateTime hr_hora)
        {

            TX_NUM_DEC = tx_num_dec;
            TX_STATUS = tx_status;
            TX_CANAL = tx_canal;
            DT_DATA = dt_data;
            HR_HORA = hr_hora;
            
        }

        [Key]
        [Identity]
        public int CD_HIST { get; set; }
        public string TX_NUM_DEC { get; set; }
        public string TX_STATUS { get; set; }
        public string TX_CANAL { get; set; }
        public DateTime DT_DATA { get; set; }
        public DateTime HR_HORA { get; set; }

    }
}
