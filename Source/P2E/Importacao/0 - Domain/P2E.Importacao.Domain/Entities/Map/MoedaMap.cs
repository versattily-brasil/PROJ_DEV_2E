using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class MoedaMap : ClassMapper<Moeda>
    {
        public MoedaMap()
        {
            Table("TB_MOEDA");

            Map(p => p.CD_MOEDA).Column("CD_MOEDA").Key(KeyType.Identity);
            Map(p => p.TX_COD_IMP).Column("TX_COD_IMP");
            Map(p => p.TX_DESCRICAO).Column("TX_DESCRICAO");
            Map(p => p.TX_COD_EXP).Column("TX_COD_EXP");
            
        }
    }
}
