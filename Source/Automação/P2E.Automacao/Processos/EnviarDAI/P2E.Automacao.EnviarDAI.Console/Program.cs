using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.EnviarDAI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = new P2E.Automacao.EnviarDAI.Lib.Work();
            cc.Executar();
        }
    }
}
