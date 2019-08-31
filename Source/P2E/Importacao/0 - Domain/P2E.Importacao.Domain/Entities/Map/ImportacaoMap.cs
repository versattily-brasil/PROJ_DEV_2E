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
            Map(p => p.NR_NUM_DEC).Column("NR_NUM_DEC");
            Map(p => p.TX_STATUS).Column("TX_STATUS");
            Map(p => p.TX_CANAL).Column("TX_CANAL");
            Map(p => p.DT_DATA_DES).Column("DT_DATA_DES");
            Map(p => p.VL_MULTA).Column("VL_MULTA");
            Map(p => p.TX_NOME_FISCAL).Column("TX_NOME_FISCAL");
            Map(p => p.DT_DATA_CANAL).Column("DT_DATA_CANAL");
            Map(p => p.DT_DATA_DISTR).Column("DT_DATA_DISTR");
        }
    }
}
