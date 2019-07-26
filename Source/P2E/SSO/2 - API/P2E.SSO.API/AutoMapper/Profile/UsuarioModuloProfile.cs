using AutoMapper;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.API.AutoMapper
{
    public class UsuarioModuloProfile : Profile
    {
        public UsuarioModuloProfile()
        {
            CreateMap<UsuarioModulo, UsuarioModuloVM>(MemberList.None);
            CreateMap<UsuarioModuloVM, UsuarioModulo>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
