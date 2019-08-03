using AutoMapper;
using P2E.Main.UI.Web.Models.SSO.Operacao;
using P2E.SSO.Domain.Entities;

namespace P2E.Main.UI.Web.AutoMapper
{
    public class OperacaoProfile : Profile
    {
        public OperacaoProfile()
        {
            CreateMap<Operacao, OperacaoViewModel>(MemberList.None);
            CreateMap<OperacaoViewModel, Operacao>(MemberList.Source);
            AllowNullDestinationValues = true;
        }
    }
}
