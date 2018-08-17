#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Index
{
    internal class CreateIndexStatementBuilder : ISqlStatementBuilder<CreateIndexStatementCollection>
    {
        #region Private Fields

        private readonly IEntityModel _model;
        private readonly IEntityMapping _entityMapping;

        #endregion

        #region Constructor(s)

        public CreateIndexStatementBuilder( IEntityModel model, IEntityMapping entityMapping )
        {
            _model = model;
            _entityMapping = entityMapping;
        }

        #endregion

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
                    ColumnName = indexMapping.PropertyName, // FIXME:
                    Order = indexMapping.Order
                } );
            }

            return new CreateIndexStatementCollection( createIndexStatements.Values );
        }

        private string GetIndexName( IIndexMapping index )
        {
            return IndexNameCreator.CreateName( _entityMapping.TableName, index.PropertyName /* fixme */ );
        }
    }
}
