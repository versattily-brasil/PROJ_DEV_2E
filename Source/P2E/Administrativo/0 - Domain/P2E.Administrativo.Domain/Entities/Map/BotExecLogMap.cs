using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Administrativo.Domain.Entities.Map
{
    public class BotExecLogMap : ClassMapper<BotExecLog>
    {
        public BotExecLogMap()
        {
            Table("TB_BOT_EXEC_LOG");

            Map(p => p.CD_BOT_EXEC_LOG).Column("CD_BOT_EXEC_LOG").Key(KeyType.Identity);
            Map(p => p.CD_BOT_EXEC).Column("CD_BOT_EXEC");
            Map(p => p.OP_TIPO_LOG).Column("OP_TIPO_LOG");
            Map(p => p.TX_MENSAGEM).Column("TX_MENSAGEM");
            Map(p => p.DT_DATAHORA_REG).Column("DT_DATAHORA_REG");
        }
    }
}
