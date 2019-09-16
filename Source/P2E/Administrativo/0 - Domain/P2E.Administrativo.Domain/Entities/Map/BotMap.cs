using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Administrativo.Domain.Entities.Map
{
    public class BotMap : ClassMapper<Bot>
    {
        public BotMap()
        {
            Table("TB_BOT");

            Map(p => p.CD_BOT).Column("CD_BOT").Key(KeyType.Identity);
            Map(p => p.OP_ATIVO).Column("OP_ATIVO");
            Map(p => p.TX_NOME).Column("TX_NOME");
            Map(p => p.TX_DESCRICAO).Column("TX_DESCRICAO");
        }
    }
}
