using FluentValidator;
using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Main.Domain.Entities
{
    [Table("TB_AUX_TAB")]
    public class TabelaAuxiliar : Notifiable
    {
        public TabelaAuxiliar()
        {

        }

        public TabelaAuxiliar(string tx_tabela, int op_ativa, string cd_tbl_pai, string tx_campo_ref)
        {
            TX_TABELA = tx_tabela;
            OP_ATIVA = op_ativa;
            CD_TBL_PAI = cd_tbl_pai;
            TX_CAMPO_REF = tx_campo_ref;
        }

        [Key]
        [Identity]
        public string TX_TABELA { get; private set; }
        public int OP_ATIVA { get; private set; }
        public string CD_TBL_PAI { get; private set; }
        public string TX_CAMPO_REF { get; private set; }

        public bool IsValid()
        {
            //if (string.IsNullOrEmpty(CD_TBL_PAI))
            //    AddNotification("CD_TBL_PAI", $"CD_TBL_PAI é um campo obrigatório.");

            return Valid;
        }

        public override string ToString() => $"{CD_TBL_PAI.ToString()}";
    }
}
