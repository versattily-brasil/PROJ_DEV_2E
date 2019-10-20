using AutoMapper;
using P2E.Importacao.Domain.Entities;
using P2E.Main.UI.Web.Models.IMP.Moeda;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class MoedaProfile : Profile
    {
        public MoedaProfile()
        {
            CreateMap<Moeda, MoedaViewModel>(MemberList.None);
            CreateMap<MoedaViewModel, Moeda>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
