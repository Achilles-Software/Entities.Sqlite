#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
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

        private IEnumerable<IColumnMapping> Properties => _entityMapping.ColumnMappings.Where( p => !p.IsKey ).ToList();
        private IEnumerable<IColumnMapping> KeyMembers => _entityMapping.ColumnMappings.Where(p => p.IsKey ).ToList();

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
