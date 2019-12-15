using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{
    public class CEMercanteItensMap : ClassMapper<CEMercanteItens>
    {
        public CEMercanteItensMap()
        {
            Table( "TB_CE_MERCANTE_ITENS" );

            Map( p => p.CD_CE_ITEM ).Column( "CD_CE_ITEM" ).Key( KeyType.Identity );
            Map( p => p.TX_NRO_CE ).Column( "TX_NRO_CE" );
            Map( p => p.TX_CODIGO ).Column( "TX_CODIGO" );
            Map( p => p.TX_TIPO ).Column( "TX_TIPO" );
            Map( p => p.VL_PESO ).Column( "VL_PESO" );
            Map( p => p.TX_PENDENTE ).Column( "TX_PENDENTE" );
            Map( p => p.TX_DETALHAMENTO ).Column( "TX_DETALHAMENTO" );

        }
    }
}
