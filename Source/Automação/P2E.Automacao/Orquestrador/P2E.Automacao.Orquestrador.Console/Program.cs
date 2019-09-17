using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var work = new P2E.Automacao.Orquestrador.Lib.Work();
            await work.ExecutarAsync();
        }
    }
}
