using AutoMapper;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.API.AutoMapper
{
    public class ModuloProfile : Profile
    {
        public ModuloProfile()
        {
            CreateMap<TB_MOD, ModuloVM>(MemberList.None);
            CreateMap<ModuloVM, TB_MOD>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
