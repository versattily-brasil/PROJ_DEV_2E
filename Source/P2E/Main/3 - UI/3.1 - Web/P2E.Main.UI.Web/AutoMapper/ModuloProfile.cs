using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Modulo;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class ModuloProfile : Profile
    {
        public ModuloProfile()
        {
            CreateMap<Modulo, ModuloViewModel>(MemberList.None);
            CreateMap<ModuloViewModel, Modulo>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
