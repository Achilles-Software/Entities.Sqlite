#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class CreateTableStatementBuilder : ISqlStatementBuilder<CreateTableStatement>
    {
        private readonly IEntityMapping EntityMapping;

        public CreateTableStatementBuilder( IEntityMapping EntityMapping )
        {
            this.EntityMapping = EntityMapping;
        }

        public CreateTableStatement BuildStatement()
        {
            var keyMembers = EntityMapping.ColumnMappings.Where( p => p.IsKey ).ToList();

            // Only create a CompositePrimaryKeyStatement if there is a composite primary key.
            // If there is just one key member this is handled using a constraint.
            CompositePrimaryKeyStatement compositePrimaryKeyStatement = null;

            if ( keyMembers.Count > 1 )
            {
                compositePrimaryKeyStatement = new CompositePrimaryKeyStatementBuilder( keyMembers ).BuildStatement();
            }

            var simpleColumnCollection = new ColumnStatementCollectionBuilder( EntityMapping.ColumnMappings, keyMembers ).BuildStatement();
            
            var foreignKeyCollection = new ForeignKeyStatementBuilder( EntityMapping.ForeignKeyMappings ).BuildStatement();

            var columnStatements = new List<ISqlStatement>();
            columnStatements.AddRange( simpleColumnCollection );
            columnStatements.AddIfNotNull( compositePrimaryKeyStatement );
            columnStatements.AddRange( foreignKeyCollection );

            return new CreateTableStatement
            {
                TableName = NameCreator.EscapeName( EntityMapping.TableName ),
                ColumnStatementCollection = new ColumnStatementCollection( columnStatements )
            };
        }
    }
}
