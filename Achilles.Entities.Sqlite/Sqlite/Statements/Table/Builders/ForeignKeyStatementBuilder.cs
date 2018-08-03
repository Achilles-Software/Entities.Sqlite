#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class ForeignKeyStatementBuilder : ISqlStatementBuilder<ColumnStatementCollection>
    {
        private readonly IEnumerable<SqliteAssociationType> associationTypes;

        public ForeignKeyStatementBuilder( IEnumerable<SqliteAssociationType> associationTypes )
        {
            this.associationTypes = associationTypes;
        }

        public ColumnStatementCollection BuildStatement()
        {
            var columnDefStatement = new ColumnStatementCollection( GetForeignKeyStatements().ToList() );

            return columnDefStatement;
        }

        private IEnumerable<ForeignKeyStatement> GetForeignKeyStatements()
        {
            foreach ( var associationType in associationTypes )
            {
                yield return new ForeignKeyStatement
                {
                    ForeignKey = associationType.ForeignKey,
                    ForeignTable = associationType.FromTableName,
                    ForeignPrimaryKey = associationType.ForeignPrimaryKey,
                    CascadeDelete = associationType.CascadeDelete
                };
            }
        }
    }
}
