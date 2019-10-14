using DapperExtensions.Mapper;

namespace P2E.Administrativo.Domain.Entities.Map
{
    public class AgendaBotMap : ClassMapper<AgendaBot>
    {
        public AgendaBotMap()
        {
            Table("TB_AGENDA_BOT");

            Map(p => p.CD_AGENDA_BOT).Column("CD_AGENDA_BOT").Key(KeyType.Identity);
            Map(p => p.CD_AGENDA).Column("CD_AGENDA");
            Map(p => p.CD_BOT).Column("CD_BOT");
            Map(p => p.NR_ORDEM_EXEC).Column("NR_ORDEM_EXEC");
            Map(p => p.TX_PARAM_EXEC).Column("TX_PARAM_EXEC");
        }
    }
}
