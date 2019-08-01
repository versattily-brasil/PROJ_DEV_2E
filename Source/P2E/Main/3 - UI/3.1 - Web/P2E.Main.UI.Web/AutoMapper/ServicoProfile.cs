using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Servico;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class ServicoProfile : Profile
    {
        public ServicoProfile()
        {
            CreateMap<Servico, ServicoViewModel>(MemberList.None);
            CreateMap<ServicoViewModel, Servico>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
