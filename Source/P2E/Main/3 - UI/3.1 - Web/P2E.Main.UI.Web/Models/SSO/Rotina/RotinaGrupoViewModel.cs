using P2E.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaGrupoViewModel
    {
        public int CD_ROT_GRP { get; set; }

        [Required]        
        public int CD_ROT { get; set; }

        [Required]
        public int CD_GRP { get; set; }

        [Required]
        public int CD_OPR { get; set; }
    }
}
