using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Usuario;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            AllowNullDestinationValues = true;
        }
    }
}
