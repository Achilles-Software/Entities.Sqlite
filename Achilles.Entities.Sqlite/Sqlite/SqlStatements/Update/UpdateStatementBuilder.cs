#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Update
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

            var keyMembers = _entityMapping.ColumnMappings.Where( p => p.IsKey ).ToList();
            var pk = keyMembers.First();
            var pkValue = _entityMapping.GetPropertyValue( _entity, pk.MemberName );

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
