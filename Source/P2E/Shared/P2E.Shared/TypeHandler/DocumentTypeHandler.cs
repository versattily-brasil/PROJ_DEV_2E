using Dapper;
using P2E.Shared.ValuesObject;
using System.Data;

namespace P2E.Shared.TypeHandler
{
    public class DocumentTypeHandler : SqlMapper.TypeHandler<Document>
    {
        public override Document Parse(object value)
        {
            return Document.FromString(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, Document value)
        {
            parameter.Value = value.ToString();
        }
    }
}
