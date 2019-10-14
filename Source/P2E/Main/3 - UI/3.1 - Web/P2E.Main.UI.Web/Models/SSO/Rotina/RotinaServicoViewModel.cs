using System.ComponentModel.DataAnnotations;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaServicoViewModel
    {
        public int CD_ROT_SRV { get; set; }

        [Required]
        public int CD_ROT { get; set; }

        [Required]
        public int CD_SRV { get; set; }
    }
}
