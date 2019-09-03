using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2E.Importacao.Domain.Entities.Map
{   
    public class HistoricoMap : ClassMapper<Historico>
    {
        public HistoricoMap()
        {
            Table("TB_HIST");

            Map(p => p.CD_HIST).Column("CD_HIST").Key(KeyType.Identity);
            Map(p => p.TX_NUM_DEC).Column("TX_NUM_DEC");
            Map(p => p.TX_STATUS).Column("TX_STATUS");
            Map(p => p.TX_CANAL).Column("TX_CANAL");
            Map(p => p.DT_DATA).Column("DT_DATA");
            Map(p => p.HR_HORA).Column("HR_HORA");
            

        }
    }
}
