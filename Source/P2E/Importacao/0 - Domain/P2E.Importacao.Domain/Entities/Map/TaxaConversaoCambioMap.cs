using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class TaxaConversaoCambioMap : ClassMapper<TaxaConversaoCambio>
    {
        public TaxaConversaoCambioMap()
        {
            Table("TB_TAXA_CONV_CAMBIO");

            Map(p => p.CD_TAXA_CAMBIO).Column("CD_TAXA_CAMBIO").Key(KeyType.Identity);
            Map(p => p.TX_MOEDA).Column("TX_MOEDA");
            Map(p => p.TX_DESCRICAO).Column("TX_DESCRICAO");
            Map(p => p.DT_INICIO_VIGENCIA).Column("DT_INICIO_VIGENCIA");
            Map(p => p.DT_FIM_VIGENCIA).Column("DT_FIM_VIGENCIA");
            Map(p => p.VL_TAXA_CONVERSAO).Column("VL_TAXA_CONVERSAO");
        }
    }
}
