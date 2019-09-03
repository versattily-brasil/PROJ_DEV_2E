using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.EnviarPLI.Lib.Entidades.EstruturaPLI
{
    [Serializable]
    public class Importador
    {
        public int tipoImportador { get; set; }
        public long cdImportador { get; set; }
        public List<ItemMatriz> itemMatriz { get; set; }
    }
}
