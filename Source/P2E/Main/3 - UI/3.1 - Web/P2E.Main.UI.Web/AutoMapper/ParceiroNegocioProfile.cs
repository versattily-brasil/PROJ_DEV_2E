using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.ParceiroNegocio;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
