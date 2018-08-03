#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Update
{
    internal class UpdateStatementBuilder : ISqlStatementBuilder<UpdateStatement>
    {
        private object _entity;
        private readonly IEntityMapping _entityMapping;
        private SqlParameterCollection _parameters;

        public UpdateStatementBuilder( object entity, IEntityMapping entityMapping )
        {
            _entity = entity;
            _entityMapping = entityMapping;

            _parameters = new SqlParameterCollection();
        }

        public UpdateStatement BuildStatement()
        {
            var columnSetCollection = new ColumnSetStatementCollectionBuilder( _entity, _entityMapping, _parameters ).BuildStatement();
            
            var columnSetStatements = new List<ISqlStatement>();
            columnSetStatements.AddRange( columnSetCollection );

            var keyMembers = _entityMapping.PropertyMappings.Where( p => p.IsKey ).ToList();
            var pk = keyMembers.First();
            var pkValue = _entityMapping.GetPropertyValue( _entity, pk.PropertyName );

            var pkParameter = _parameters.Add( pkValue );

            return new UpdateStatement
            {
                TableName = NameCreator.EscapeName( _entityMapping.TableName ),
                ColumnSetStatementCollection = new ColumnSetStatementCollection( columnSetStatements ),
                PkColumn = pk.ColumnName,
                PkParameter = pkParameter.Name,
                Parameters = _parameters
            };
        }
    }
}
