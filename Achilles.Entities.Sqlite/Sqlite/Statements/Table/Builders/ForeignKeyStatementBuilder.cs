#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class ForeignKeyStatementBuilder : ISqlStatementBuilder<ColumnStatementCollection>
    {
        private readonly IEnumerable<IForeignKeyMapping> _foreignKeyMappings;

        public ForeignKeyStatementBuilder( IEnumerable<IForeignKeyMapping> foreignKeyMappings )
        {
            _foreignKeyMappings = foreignKeyMappings;
        }

        public ColumnStatementCollection BuildStatement()
        {
            var columnDefStatement = new ColumnStatementCollection( GetForeignKeyStatements().ToList() );

            return columnDefStatement;
        }

        private IEnumerable<ForeignKeyStatement> GetForeignKeyStatements()
        {
            foreach ( var foreignKeyMapping in _foreignKeyMappings )
            {
                yield return new ForeignKeyStatement
                {
                    //ForeignKey = foreignKeyMapping.ForeignKey,
                    //ForeignTable = foreignKeyMapping.FromTableName,
                    //ForeignPrimaryKey = foreignKeyMapping.ForeignPrimaryKey,
                    //CascadeDelete = foreignKeyMapping.CascadeDelete
                };
            }
        }
    }
}
