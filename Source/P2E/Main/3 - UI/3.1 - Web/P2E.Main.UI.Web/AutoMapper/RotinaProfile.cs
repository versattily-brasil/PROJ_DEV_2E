using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class RotinaProfile : Profile
    {
        public RotinaProfile()
        {
            CreateMap<Rotina, RotinaViewModel>(MemberList.None);
            CreateMap<RotinaViewModel, Rotina>(MemberList.None);

            CreateMap<RotinaGrupoOperacao, RotinaGrupoViewModel>(MemberList.None);
            CreateMap<RotinaGrupoViewModel, RotinaGrupoOperacao>(MemberList.None);
            AllowNullDestinationValues = true;
        }
    }
}
