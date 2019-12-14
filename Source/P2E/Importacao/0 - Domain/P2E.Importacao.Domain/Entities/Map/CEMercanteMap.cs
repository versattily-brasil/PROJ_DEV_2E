using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class CEMercanteMap : ClassMapper<CEMercante>
    {
        public CEMercanteMap()
        {
            Table( "TB_CE_MERCANTE" );

            Map( p => p.CD_CE ).Column( "CD_CE" ).Key( KeyType.Identity );
            Map( p => p.TX_CE_MERCANTE ).Column( "TX_CE_MERCANTE" );
            Map( p => p.TX_NUM_BL ).Column( "TX_NUM_BL" );
            Map( p => p.TX_TIPO ).Column( "TX_TIPO" );
            Map( p => p.TX_NUM_MANIFESTO ).Column( "TX_NUM_MANIFESTO" );
            Map( p => p.DT_DATA_OPERACAO ).Column( "DT_DATA_OPERACAO" );
            
        }
    }
}
