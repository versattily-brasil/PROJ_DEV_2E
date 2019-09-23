using DapperExtensions.Mapper;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class ImportacaoMap : ClassMapper<TBImportacao>
    {
        public ImportacaoMap()
        {
            Table("TB_IMP");

            Map(p => p.CD_IMP).Column("CD_IMP").Key(KeyType.Identity);
            Map(p => p.NUM_PI).Column("NUM_PI");
            Map(p => p.CD_IMP_STATUS).Column("CD_IMP_STATUS");
            Map(p => p.CD_IMP_CANAL).Column("CD_IMP_CANAL");
            Map(p => p.TX_NUM_DEC).Column("TX_NUM_DEC");
            Map(p => p.DT_DATA_DES).Column("DT_DATA_DES");
            Map(p => p.VL_MULTA).Column("VL_MULTA");
            Map(p => p.TX_NOME_FISCAL).Column("TX_NOME_FISCAL");
            Map(p => p.DT_DATA_CANAL).Column("DT_DATA_CANAL");
            Map(p => p.DT_DATA_DISTR).Column("DT_DATA_DISTR");
            Map(p => p.TX_DOSSIE).Column("TX_DOSSIE");
            Map(p => p.DT_DATA_DOSS).Column("DT_DATA_DOSS");
            Map(p => p.UF_DI).Column("UF_DI");
            Map(p => p.DT_DATA_EXO_ICMS).Column("DT_DATA_EXO_ICMS");
            Map(p => p.OP_EXTRATO_PDF).Column("OP_EXTRATO_PDF");
            Map(p => p.OP_EXTRATO_XML).Column("OP_EXTRATO_XML");
            Map(p => p.OP_EXTRATO_RETIF).Column("OP_EXTRATO_RETIF");
            Map(p => p.OP_TELA_DEBITO).Column("OP_TELA_DEBITO");
            Map(p => p.OP_STATUS_DESEMB).Column("OP_STATUS_DESEMB");

        }
    }
}
