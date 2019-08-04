using P2E.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace P2E.Main.UI.Web.Models.SSO.Rotina
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaViewModel
    {
        public int CD_ROT { get; set; }

        [Required]
        [MaxLength(50)]
        public string TX_NOME { get; set; }

        [Required]
        public string TX_DSC { get; set; }

        [Required]
        public eTipoRotina OP_TIPO { get; set; }
    }
}
