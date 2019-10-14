using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.ParceiroNegocio;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class ParceiroNegocioProfile : Profile
    {
        public ParceiroNegocioProfile()
        {
            CreateMap<ParceiroNegocio, ParceiroNegocioViewModel>(MemberList.None);
            CreateMap<ParceiroNegocioViewModel, ParceiroNegocio>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
