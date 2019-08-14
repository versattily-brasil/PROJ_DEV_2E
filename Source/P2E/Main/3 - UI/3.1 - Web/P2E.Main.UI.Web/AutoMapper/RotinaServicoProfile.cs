using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Rotina;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class RotinaServicoProfile : Profile
    {
        public RotinaServicoProfile()
        {
            CreateMap<Rotina, RotinaServicoViewModel>(MemberList.None);
            CreateMap<RotinaServicoViewModel, Rotina>(MemberList.None);
            AllowNullDestinationValues = true;
        }
    }
}
