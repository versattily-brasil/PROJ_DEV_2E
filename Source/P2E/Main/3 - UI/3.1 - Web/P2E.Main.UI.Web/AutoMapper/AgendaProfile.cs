using AutoMapper;
using P2E.Administrativo.Domain.Entities;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class AgendaProfile : Profile
    {
        public AgendaProfile()
        {
            CreateMap<Agenda, AgendaProfile>(MemberList.None);
            CreateMap<AgendaProfile, Agenda>(MemberList.None);

            //CreateMap<RotinaGrupoOperacao, RotinaGrupoViewModel>(MemberList.None);
            //CreateMap<RotinaGrupoViewModel, RotinaGrupoOperacao>(MemberList.None);

            //CreateMap<RotinaUsuarioOperacao, RotinaUsuarioViewModel>(MemberList.None);
            //CreateMap<RotinaUsuarioViewModel, RotinaUsuarioOperacao>(MemberList.None);

            AllowNullDestinationValues = true;
        }
    }
}
