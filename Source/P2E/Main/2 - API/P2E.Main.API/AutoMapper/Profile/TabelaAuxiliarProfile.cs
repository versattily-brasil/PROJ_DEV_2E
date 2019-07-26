using AutoMapper;
using P2E.Main.API.ViewModel;
using P2E.Main.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.API.AutoMapper
{
    public class TabelaAuxiliarProfile : Profile
    {
        public TabelaAuxiliarProfile()
        {
            CreateMap<TabelaAuxiliar, TabelaAuxiliarVM>(MemberList.None);
            CreateMap<TabelaAuxiliarVM, TabelaAuxiliar>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
