using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class RotinaGrupoProfile : Profile
    {
        public RotinaGrupoProfile()
        {
            CreateMap<Rotina, RotinaGrupoViewModel>(MemberList.None);
            CreateMap<RotinaGrupoViewModel, Rotina>(MemberList.None);
            AllowNullDestinationValues = true;
        }
    }
}
