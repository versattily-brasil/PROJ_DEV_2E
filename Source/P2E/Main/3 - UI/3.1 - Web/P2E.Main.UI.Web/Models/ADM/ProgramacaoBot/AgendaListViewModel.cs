using P2E.Shared.Model;
using System.Collections.Generic;

namespace P2E.Main.UI.Web.Models.ADM.ProgramacaoBot
{
    public class AgendaListViewModel
    {
        public AgendaListViewModel()
        {
            DataPage = new DataPage<P2E.Administrativo.Domain.Entities.Agenda>();
        }

        public string Descricao { get; set; }

        public DataPage<P2E.Administrativo.Domain.Entities.Agenda> DataPage { get; set; }

        public List<P2E.SSO.Domain.Entities.Servico> Servicos { get; set; }
        public P2E.SSO.Domain.Entities.Servico Servico { get; set; }
    }
}