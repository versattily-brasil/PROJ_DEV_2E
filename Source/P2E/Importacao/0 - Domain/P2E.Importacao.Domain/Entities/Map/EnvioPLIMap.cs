using DapperExtensions.Mapper;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class EnvioPLIMap : ClassMapper<EnvioPLI>
    {
        public EnvioPLIMap()
        {
            Table("TB_ENV_PLI");

            Map(p => p.CD_ENV_PLI).Column("CD_ENV_PLI").Key(KeyType.Identity);
            Map(p => p.DT_DATA_REG).Column("DT_DATA_REG");
            Map(p => p.NR_IMPORTADOR).Column("NR_IMPORTADOR");
            Map(p => p.DT_DATA_EXEC).Column("DT_DATA_EXEC");
            Map(p => p.TX_LOG).Column("TX_LOG");
            Map(p => p.NR_CODIGO_ENVIO).Column("NR_CODIGO_ENVIO");
        }
    }
}
