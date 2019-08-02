using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Usuario;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioViewModel>(MemberList.None);
            CreateMap<UsuarioViewModel, Usuario>(MemberList.Source);

            CreateMap<UsuarioModulo, UsuarioModuloViewModel>(MemberList.None);
            CreateMap<UsuarioModuloViewModel, UsuarioModulo>(MemberList.Source);

            CreateMap<UsuarioGrupo, UsuarioGrupoViewModel>(MemberList.None);
            CreateMap<UsuarioGrupoViewModel, UsuarioGrupo>(MemberList.Source);

            AllowNullDestinationValues = true;
        }
    }
}
