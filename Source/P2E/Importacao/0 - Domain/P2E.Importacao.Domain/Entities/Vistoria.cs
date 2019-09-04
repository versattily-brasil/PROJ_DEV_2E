using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_VIST")]
    public class Vistoria
    {
        public Vistoria()
        {

        }

        public Vistoria(int cd_imp, string tx_desc)
        {
            CD_IMP = cd_imp;
            TX_DESC = tx_desc;
        }

        [Key]
        [Identity]
        public int CD_VIST { get; set; }
        public int CD_IMP { get; set; }
        public string TX_DESC { get; set; }
    }
}
