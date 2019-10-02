using AutoMapper;
using P2E.Importacao.Domain.Entities;
using P2E.Main.UI.Web.Models.IMP.DetalheNCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class DetalheNCMProfile : Profile
    {
        public DetalheNCMProfile()
        {
            CreateMap<DetalheNCM, DetalheNCMViewModel>(MemberList.None);
            CreateMap<DetalheNCMViewModel, DetalheNCM>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
