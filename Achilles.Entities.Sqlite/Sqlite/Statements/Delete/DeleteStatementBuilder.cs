#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Statements;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Delete
{
    internal class DeleteStatementBuilder : ISqlStatementBuilder<DeleteStatement>
    {
        private object _entity;
        private readonly IEntityMapping _entityMapping;
        private SqlParameterCollection _parameters;

        public DeleteStatementBuilder( object entity, IEntityMapping entityMapping )
        {
            _entity = entity;
            _entityMapping = entityMapping;

            _parameters = new SqlParameterCollection();
        }

        public DeleteStatement BuildStatement()
        {
            var keyMembers = _entityMapping.PropertyMappings.Where( p => p.IsKey ).ToList();

            var pk = keyMembers.First();
            var pkValue = _entityMapping.GetPropertyValue( _entity, pk.PropertyName );

            var pkParameter = _parameters.Add( pkValue );

            return new DeleteStatement
            {
                TableName = NameCreator.EscapeName( _entityMapping.TableName ),
                PkName = pk.ColumnName,
                PkParameterName = pkParameter.Name,
                Parameters = _parameters
            };
        }
    }
}
