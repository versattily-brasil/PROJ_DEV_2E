using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Automacao.Orquestrador.Lib.Entidades.Map
{
    public class AgendaExecMap : ClassMapper<AgendaExec>
    {
        public AgendaExecMap()
        {
            Table("TB_AGENDA_EXEC");

            Map(p => p.CD_AGENDA_EXEC).Column("CD_AGENDA_EXEC").Key(KeyType.Identity);
            Map(p => p.CD_AGENDA).Column("CD_AGENDA");
            Map(p => p.DT_FIM_EXEC).Column("DT_FIM_EXEC");
            Map(p => p.DT_INICIO_EXEC).Column("DT_INICIO_EXEC");
            Map(p => p.OP_STATUS_AGENDA_EXEC).Column("OP_STATUS_AGENDA_EXEC");
        }
    }
}
