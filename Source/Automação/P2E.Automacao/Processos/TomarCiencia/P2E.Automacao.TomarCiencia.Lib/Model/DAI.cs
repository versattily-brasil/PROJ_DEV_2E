using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2E.Automacao.TomarCiencia.Lib.Model.Enums;

namespace P2E.Automacao.TomarCiencia.Lib.Model
{
    public class DAI
    {
        // Inscrição Estadual
        public string InscricaoEstadual { get; set; }
        public string Numero { get; set; }
        public DateTime Data { get; set; }
        public eCanal Canal { get; set; }
        // Registra se Tomou Ciência ou não
        public bool TomouCiencia { get; set; }
    }
}
