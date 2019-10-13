using DapperExtensions.Mapper;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class HistoricoMap : ClassMapper<Historico>
    {
        public HistoricoMap()
        {
            Table("TB_HIST");

            Map(p => p.CD_HIST).Column("CD_HIST").Key(KeyType.Identity);
            Map(p => p.CD_IMP).Column("CD_IMP");
            Map(p => p.CD_IMP_STATUS).Column("CD_IMP_STATUS");
            Map(p => p.CD_IMP_CANAL).Column("CD_IMP_CANAL");
            Map(p => p.TX_NUM_DEC).Column("TX_NUM_DEC");
            Map(p => p.DT_DATA).Column("DT_DATA");
            Map(p => p.HR_HORA).Column("HR_HORA");


        }
    }
}
