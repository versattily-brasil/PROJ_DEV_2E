using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class DetalheNCMMap : ClassMapper<DetalheNCM>
    {
        public DetalheNCMMap()
        {
            Table("TB_DETALHE_NCM");

            Map(p => p.CD_DET_NCM).Column("CD_DET_NCM").Key(KeyType.Identity);
            Map(p => p.TX_SFNCM_CODIGO).Column("TX_SFNCM_CODIGO");
            Map(p => p.TX_SFNCM_DETALHE).Column("TX_SFNCM_DETALHE");
            Map(p => p.TX_SFNCM_DESCRICAO).Column("TX_SFNCM_DESCRICAO");
        }
    }
}
