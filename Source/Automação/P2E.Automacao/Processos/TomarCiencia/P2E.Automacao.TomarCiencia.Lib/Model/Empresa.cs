using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.TomarCiencia.Lib.Model
{
    public class Empresa
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public List<string> IncricoesEstaduais { get; set; } 
    }
}
