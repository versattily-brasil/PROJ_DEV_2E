using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.Models.IMP.DetalheNCM
{
    [Serializable]
    public class DetalheNCMViewModel
    {
        public int CD_DET_NCM { get; set; }
        public string TX_SFNCM_CODIGO { get; set; }
        public string TX_SFNCM_DETALHE { get; set; }
        public string TX_SFNCM_DESCRICAO { get; set; }

        public List<DetalheNCMViewModel> RotinasViewModel { get; set; }
    }
}
