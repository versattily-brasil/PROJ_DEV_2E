using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Entidades
{
    public class TriagemBot
    {
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
    }
}
