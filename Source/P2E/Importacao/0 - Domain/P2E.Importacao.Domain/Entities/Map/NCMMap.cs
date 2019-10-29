using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class NCMMap : ClassMapper<NCM>
    {
        public NCMMap()
        {
            Table("TB_NCM");

            Map(p => p.CD_NCM).Column("CD_NCM").Key(KeyType.Identity);
            Map(p => p.TX_NRO_NCM).Column("TX_NRO_NCM");
            Map(p => p.TX_DESCRICAO).Column("TX_DESCRICAO");
        }
    }
}
