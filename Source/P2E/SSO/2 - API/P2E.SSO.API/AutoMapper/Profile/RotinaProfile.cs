using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;

namespace P2E.SSO.API.AutoMapper
{
    public class RotinaProfile : Profile
    {
        public RotinaProfile()
        {
            CreateMap<Rotina, RotinaVM>(MemberList.None);
            CreateMap<RotinaVM, Rotina>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
