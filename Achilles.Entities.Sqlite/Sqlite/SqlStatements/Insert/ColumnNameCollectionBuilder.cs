#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Sqlite.SqlStatements.Common;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Insert
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
