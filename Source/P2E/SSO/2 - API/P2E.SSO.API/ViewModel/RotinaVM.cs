using P2E.Shared.Enum;

namespace P2E.SSO.API.ViewModel
{
    /// <summary>
    /// Classe de apresentação de Rotina na View
    /// </summary>
    public class RotinaVM
    {
        public int CD_ROT { get; set; }
        public string TX_NOME { get; set; }
        public string TX_DSC { get; set; }        
        public eTipoRotina OP_TIPO { get; set; }
    }
}
