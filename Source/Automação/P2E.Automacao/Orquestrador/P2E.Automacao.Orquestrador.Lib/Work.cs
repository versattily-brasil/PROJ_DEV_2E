using P2E.Automacao.Orquestrador.Lib.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace P2E.Automacao.Orquestrador.Lib
{
    public class Work
    {
        protected List<Agenda> Agendas;
        private string _urlApiBase;

        public Work()
        {
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            // obter agenda do dia ou com data não preenchida com status ativo != 0
            await CarregarAgendasAsync();

            foreach (var agenda in Agendas)
            {
                if (agenda.OP_STATUS == Util.Enum.eStatusExec.Pendente)
                {

                }
            }
        }

        private async Task CarregarAgendasAsync()
        {
            string data = DateTime.Today.ToString("dd-MM-yyyy", null);
            // monta url para api de importação.
            string url = _urlApiBase + $"adm/v1/agenda/por-data/{data}";

            // realiza a requisição para a api
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);

                // recupera os registros.
                Agendas = await result.Content.ReadAsAsync<List<Agenda>>();
            }
        }
    }
}
