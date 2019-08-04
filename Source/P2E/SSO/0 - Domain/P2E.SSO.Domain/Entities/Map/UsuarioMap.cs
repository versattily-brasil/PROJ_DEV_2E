using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.SSO.Domain.Entities.Map
{
    public class UsuarioMap : ClassMapper<Usuario>
    {
        public UsuarioMap()
        {
            Table("TB_USR");

            Map(p => p.CD_USR).Column("CD_USR").Key(KeyType.Identity);
            Map(p => p.TX_NOME).Column("TX_NOME");
            Map(p => p.TX_LOGIN).Column("TX_LOGIN");
            Map(p => p.TX_SENHA).Column("TX_SENHA");
            Map(p => p.OP_STATUS).Column("OP_STATUS");
        }
    }
}
