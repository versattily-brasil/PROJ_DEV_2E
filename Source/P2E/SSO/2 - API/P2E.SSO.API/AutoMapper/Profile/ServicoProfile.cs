using AutoMapper;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.AutoMapper
{
    public class ServicoProfile : Profile
    {
        public ServicoProfile()
        {
            CreateMap<Servico, ServicoVM>(MemberList.None);
            CreateMap<ServicoVM, Servico>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
