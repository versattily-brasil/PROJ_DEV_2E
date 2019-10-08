using System;
using System.Threading.Tasks;

namespace P2E.Automacao.Processos.TaxaConversaoCambio.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lib = new Lib.Work();
                await lib.ExecutarAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
