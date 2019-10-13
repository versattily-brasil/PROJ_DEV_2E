using DapperExtensions.Mapper;

namespace P2E.Administrativo.Domain.Entities.Map
{
    public class AgendaExecLogMap : ClassMapper<AgendaExecLog>
    {
        public AgendaExecLogMap()
        {
            Table("TB_AGENDA_EXEC_LOG");

            Map(p => p.CD_AGENDA_EXEC_LOG).Column("CD_AGENDA_EXEC_LOG").Key(KeyType.Identity);
            Map(p => p.CD_AGENDA_EXEC).Column("CD_AGENDA_EXEC");
            Map(p => p.OP_TIPO_LOG).Column("OP_TIPO_LOG");
            Map(p => p.TX_MENSAGEM).Column("TX_MENSAGEM");
            Map(p => p.DT_DATAHORA_REG).Column("DT_DATAHORA_REG");

        }
    }
}
