using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.ComprovanteImportacao.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = new P2E.Automacao.ComprovanteImportacao.Lib.Work();
            cc.Executar(args);
        }
    }
}
