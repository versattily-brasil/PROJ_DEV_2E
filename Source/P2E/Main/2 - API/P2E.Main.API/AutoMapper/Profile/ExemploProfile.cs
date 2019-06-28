using AutoMapper;
using P2E.Main.API.ViewModel;
using P2E.Main.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.API.AutoMapper
{
    public class ExemploProfile : Profile
    {
        public ExemploProfile()
        {
            CreateMap<Exemplo, ExemploVM>(MemberList.None);
            CreateMap<ExemploVM, Exemplo>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
