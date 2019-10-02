using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Models.IMP.DetalheNCM
{
    public class DetalheNCMListViewModel
    {
        public DetalheNCMListViewModel()
        {
            DataPage = new DataPage<P2E.Importacao.Domain.Entities.DetalheNCM>();
        }

        public int CD_DET_NCM { get; set; }
        public string TX_SFNCM_CODIGO { get; set; }
        public string TX_SFNCM_DETALHE { get; set; }
        public string TX_SFNCM_DESCRICAO { get; set; }
        public DataPage<P2E.Importacao.Domain.Entities.DetalheNCM> DataPage { get; set; }
    }
}
