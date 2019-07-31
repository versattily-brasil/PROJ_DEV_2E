using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Grupo;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class GrupoProfile : Profile
    {
        public GrupoProfile()
        {
            CreateMap<Grupo, GrupoViewModel>(MemberList.None);
            CreateMap<GrupoViewModel, Grupo>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
