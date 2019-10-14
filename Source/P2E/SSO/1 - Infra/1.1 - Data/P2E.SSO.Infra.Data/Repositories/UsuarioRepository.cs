using DapperExtensions;
using MicroOrm.Dapper.Repositories;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using P2E.SSO.Infra.Data.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.SSO.Infra.Data.Repositories
{
    public class UsuarioRepository : DapperRepository<Usuario>, IUsuarioRepository
    {
        private readonly SSOContext _ssoContext;

        public UsuarioRepository(SSOContext ssoContext) : base(ssoContext.Connection)
        {
            _ssoContext = ssoContext;
        }

        public DataPage<Usuario> GetByPage(DataPage<Usuario> dataPage, string nome)
        {

            var sSQL = new StringBuilder();
            dataPage.OrderBy = dataPage.OrderBy ?? "tx_nome";
            var sort = new Sort() { PropertyName = dataPage.OrderBy, Ascending = !dataPage.Descending };

            #region Ordenação
            var listSort = new List<ISort>();
            listSort.Add(sort);
            #endregion

            #region Filtros
            var predicateGroup = new PredicateGroup();
            predicateGroup.Predicates = new List<IPredicate>();

            if (!string.IsNullOrEmpty(nome))
            {
                var predicate = Predicates.Field<Usuario>(p => p.TX_NOME, Operator.Like, "%" + nome.Trim() + "%", false);
                predicateGroup.Predicates.Add(predicate);
            }

            #endregion

            predicateGroup.Operator = (predicateGroup.Predicates.Count > 1 ? GroupOperator.And : GroupOperator.Or);

            dataPage.Items = _ssoContext.Connection.GetPage<Usuario>(predicateGroup, listSort, dataPage.CurrentPage - 1, dataPage.PageSize).ToList();

            dataPage.TotalItems = GetTotalRows(predicateGroup);
            return dataPage;
        }

        public int GetTotalRows(PredicateGroup predicateGroup)
        {
            return _ssoContext.Connection.GetList<Usuario>(predicateGroup).Count();
        }

        public bool ValidarDuplicidades(Usuario usuario)
        {
            if (usuario.CD_USR > 0)
            {
                if (FindAll(p => p.TX_LOGIN == usuario.TX_LOGIN && p.CD_USR != usuario.CD_USR).Any())
                {
                    usuario.AddNotification("TX_LOGIN", $"O Login de Usuário {usuario.TX_LOGIN} já está cadastrado.");
                }

                if (FindAll(p => p.TX_NOME == usuario.TX_NOME && p.CD_USR != usuario.CD_USR).Any())
                {
                    usuario.AddNotification("TX_NOME", $"O Nome {usuario.TX_NOME} já está cadastrado.");
                }
            }
            else
            {
                if (FindAll(p => p.TX_LOGIN == usuario.TX_LOGIN).Any())
                {
                    usuario.AddNotification("TX_LOGIN", $"O Login de Usuário {usuario.TX_LOGIN} já está cadastrado.");
                }

                if (FindAll(p => p.TX_NOME == usuario.TX_NOME).Any())
                {
                    usuario.AddNotification("TX_NOME", $"O Nome {usuario.TX_NOME} já está cadastrado.");
                }
            }

            return usuario.IsValid();
        }
    }
}
