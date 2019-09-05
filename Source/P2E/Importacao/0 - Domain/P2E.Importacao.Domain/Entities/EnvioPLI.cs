using MicroOrm.Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_ENV_PLI")]
    public class EnvioPLI
    {
        public EnvioPLI()
        {

        }


        [Key]
        [Identity]
        public int CD_ENV_PLI { get; set; }
        public DateTime DT_DATA_REG { get; set; }
        public int NR_IMPORTADOR { get; set; }
        public int OP_STATUS { get; set; }
        public DateTime DT_DATA_EXEC { get; set; }
        public string TX_LOG { get; set; }
        public string NR_CODIGO_ENVIO { get; set; }
    }
}
