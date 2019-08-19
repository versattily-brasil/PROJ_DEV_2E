using System;

namespace P2E.Main.UI.Web.Models.SSO.Operacao
{
    /// <summary>
    /// Classe de apresentação de Operação na View
    /// </summary>
    [Serializable]

    public class OperacaoViewModel
    {
        public int CD_OPR { get; set; }        
        public string TX_DSC { get; set; }

        public bool Check { get; set; }
    }
}
