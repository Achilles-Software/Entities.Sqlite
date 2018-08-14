#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Insert
{
    internal class ColumnParameterStatementCollectionBuilder : ISqlStatementBuilder<ColumnParameterStatementCollection>
    {
        private readonly object _entity;
        private readonly IEntityMapping _entityMapping;

        public ColumnParameterStatementCollectionBuilder( object entity, IEntityMapping entityMapping )
        {
            _entity = entity;
            _entityMapping = entityMapping;
        }

        private IEnumerable<IColumnMapping> Properties => _entityMapping.ColumnMappings.Where( p => !p.IsKey ).ToList();

        public SqlParameterCollection Parameters { get; private set; } = new SqlParameterCollection();

        public ColumnParameterStatementCollection BuildStatement()
        {
            var columnParameterStatement = new ColumnParameterStatementCollection( CreateColumnParameterStatements().ToList() );

            return columnParameterStatement;
        }

        private IEnumerable<ColumnParameterStatement> CreateColumnParameterStatements()
        {
            foreach ( var property in Properties )
            {
                var propertyValue = _entityMapping.GetPropertyValue( _entity, property.MemberName );

                var propertyParameter = Parameters.Add( propertyValue );
                
                var columnParameterStatement = new ColumnParameterStatement
                {
                    ColumnParameter = propertyParameter.Name
                };

                yield return columnParameterStatement;
            }
        }
    }
}
