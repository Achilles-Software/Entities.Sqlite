#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Sqlite.Statements.Common;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Insert
{
    internal class ColumnNameStatementCollectionBuilder : ISqlStatementBuilder<ColumnNameStatementCollection>
    {
        private readonly IEntityMapping _entityMapping;

        public ColumnNameStatementCollectionBuilder( IEntityMapping entityMapping )
        {
            _entityMapping = entityMapping;
        }

        private IEnumerable<IPropertyMapping> Properties => _entityMapping.PropertyMappings.Where( p => !p.IsKey ).ToList();
        private IEnumerable<IPropertyMapping> KeyMembers => _entityMapping.PropertyMappings.Where(p => p.IsKey ).ToList();

        public ColumnNameStatementCollection BuildStatement()
        {
            var columnDefStatement = new ColumnNameStatementCollection(CreateColumnNameStatements().ToList());

            return columnDefStatement;
        }

        private IEnumerable<ColumnNameStatement> CreateColumnNameStatements()
        {
            foreach (var property in Properties)
            {
                var columnNameStatement = new ColumnNameStatement
                {
                    ColumnName = property.ColumnName,
                };

                yield return columnNameStatement;
            }
        }
    }
}
