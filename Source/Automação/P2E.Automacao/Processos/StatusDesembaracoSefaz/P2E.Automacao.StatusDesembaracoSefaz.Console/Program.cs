using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.StatusDesembaracoSefaz.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lib = new P2E.Automacao.Processos.StatusDesembaracoSefaz.Lib.Work();
                await lib.ExecutarAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
