#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Index
{
    internal class CreateIndexStatementBuilder : ISqlStatementBuilder<CreateIndexStatementCollection>
    {
        private readonly IEntityMapping _entityMapping;

        public CreateIndexStatementBuilder( IEntityMapping entityMapping )
        {
            this._entityMapping = entityMapping;
        }

        public CreateIndexStatementCollection BuildStatement()
        {
            IDictionary<string, CreateIndexStatement> createIndexStatements = new Dictionary<string, CreateIndexStatement>();

            string tableName = NameCreator.EscapeName( _entityMapping.TableName );

            foreach ( var indexMapping in _entityMapping.IndexMappings )
            {
                string indexName = GetIndexName( indexMapping );

                // There is a single CreateIndexStatement per indexName
                if ( !createIndexStatements.TryGetValue( indexName, out CreateIndexStatement createIndexStatement ) )
                {
                    createIndexStatement = new CreateIndexStatement
                    {
                        IsUnique = indexMapping.IsUnique,
                        IndexName = indexName,
                        TableName = tableName,
                        Columns = new Collection<CreateIndexStatement.IndexColumn>()
                    };

                    createIndexStatements.Add( indexName, createIndexStatement );
                }

                createIndexStatement.Columns.Add( new CreateIndexStatement.IndexColumn
                {
                    ColumnName = indexMapping.PropertyInfo.Name,
                    Order = indexMapping.Order
                } );
            }

            return new CreateIndexStatementCollection( createIndexStatements.Values );
        }

        private string GetIndexName( IIndexMapping index )
        {
            return IndexNameCreator.CreateName( _entityMapping.TableName, index.PropertyInfo.Name );
        }
    }
}
