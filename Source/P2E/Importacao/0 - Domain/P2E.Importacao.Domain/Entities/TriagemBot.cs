using MicroOrm.Dapper.Repositories.Attributes;
using P2E.Shared.Message;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2E.Importacao.Domain.Entities
{
    [Table("TB_TRIAGEM_BOT")]
    public class TriagemBot : CustomNotifiable
    {
        public TriagemBot()
        {

        }

        public TriagemBot(string nr_di,                         
                         DateTime dt_registro,
                         int cd_par_neg,
                         int op_acomp_desp_imp,
                         int op_comprov_imp,
                         int op_exonera_icms,
                         int op_extrato_retif,
                         int op_status_desemb,
                         int op_status,
                         int op_tela_debito,
                         int op_extrato_pdf_xml)
        {
            NR_DI = nr_di;
            DT_REGISTRO = dt_registro;
            CD_PAR_NEG = cd_par_neg;
            OP_ACOMP_DESP_IMP = op_acomp_desp_imp;
            OP_COMPROV_IMP = op_comprov_imp;
            OP_EXONERA_ICMS = op_exonera_icms;
            OP_EXTRATO_RETIF = op_extrato_retif;
            OP_STATUS_DESEMB = op_status_desemb;
            OP_STATUS = op_status;
            OP_TELA_DEBITO = op_tela_debito;
            OP_EXTRATO_PDF_XML = op_extrato_pdf_xml;

            IsValid();
        }

        [Key]
        [Identity]
        public int CD_TRIAGEM { get; set; }
        public string NR_DI { get; set; }
        public DateTime? DT_REGISTRO { get; set; }
        public int CD_PAR_NEG { get; set; }
        public int OP_STATUS { get; set; }
        public int OP_ACOMP_DESP_IMP { get; set; }
        public int OP_COMPROV_IMP { get; set; }
        public int OP_EXONERA_ICMS { get; set; }
        public int OP_EXTRATO_RETIF { get; set; }
        public int OP_STATUS_DESEMB { get; set; }
        public int OP_TELA_DEBITO { get; set; }
        public int OP_EXTRATO_PDF_XML { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(NR_DI))
                AddNotification("NR_DI", $"Nro. DI é um campo obrigatório.");
            return Valid;
        }

        public override string ToString() => $"{NR_DI.ToString()}";
    }
}
