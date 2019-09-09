using System;
using System.Threading.Tasks;

namespace P2E.Automacao.AcompanharDespachos.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lib = new P2E.Automacao.AcompanharDespachos.Lib.Work();
                await lib.ExecutarAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
