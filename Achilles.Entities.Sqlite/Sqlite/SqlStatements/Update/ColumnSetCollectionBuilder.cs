#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Update
{
    internal class ColumnSetStatementCollectionBuilder : ISqlStatementBuilder<ColumnSetStatementCollection>
    {
        private readonly object _entity;
        private readonly IEntityMapping _entityMapping;

        public ColumnSetStatementCollectionBuilder( object entity, IEntityMapping entityMapping, SqlParameterCollection updateParameters )
        {
            _entity = entity;
            _entityMapping = entityMapping;

            Parameters = updateParameters;
        }

        private IEnumerable<IColumnMapping> Properties => _entityMapping.ColumnMappings.Where( p => !p.IsKey ).ToList();

        public SqlParameterCollection Parameters { get; private set; }

        public ColumnSetStatementCollection BuildStatement()
        {
            var columnSetStatement = new ColumnSetStatementCollection( CreateColumnSetStatements().ToList() );

            return columnSetStatement;
        }

        private IEnumerable<ColumnSetStatement> CreateColumnSetStatements()
        {
            foreach ( var property in Properties )
            {
                var propertyValue = _entityMapping.GetPropertyValue( _entity, property.MemberName );

                var propertyParameter = Parameters.Add( propertyValue );
                
                var columnSetStatement = new ColumnSetStatement
                {
                    ColumnName = property.ColumnName,
                    ColumnParameter = propertyParameter.Name
                };

                yield return columnSetStatement;
            }
        }
    }
}
