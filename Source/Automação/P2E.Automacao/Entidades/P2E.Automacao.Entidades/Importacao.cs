using System;

namespace P2E.Automacao.Entidades
{
    public class Importacao
    {
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
        public int OP_EXTRATO_RETIF { get; set; }
        public int OP_TELA_DEBITO { get; set; }
        public int OP_STATUS_DESEMB { get; set; }
    }
}
