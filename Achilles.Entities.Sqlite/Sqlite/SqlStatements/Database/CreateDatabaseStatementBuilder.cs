#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Modelling;
using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Sqlite.SqlStatements.Index;
using Achilles.Entities.Sqlite.SqlStatements.Table;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Database
{
    internal class CreateDatabaseStatementBuilder : ISqlStatementBuilder<CreateDatabaseStatement>
    {
        #region Fields

        private readonly IEntityModel _model;

        #endregion

        #region Constructor(s)

        public CreateDatabaseStatementBuilder( IEntityModel model )
        {
            _model = model;
        }

        #endregion

        #region Public Methods

        public CreateDatabaseStatement BuildStatement()
        {
            var createTableStatements = GetCreateTableStatements();
            var createIndexStatements = GetCreateIndexStatements();
            var createStatements = createTableStatements.Concat<ISqlStatement>( createIndexStatements );
            var createDatabaseStatement = new CreateDatabaseStatement( createStatements );

            return createDatabaseStatement;
        }

        #endregion

        #region Private Methods

        private IEnumerable<CreateTableStatement> GetCreateTableStatements()
        {
            foreach ( var entityMapping in _model.EntityMappings )
            {
                var tableStatementBuilder = new CreateTableStatementBuilder( _model, entityMapping );

                yield return tableStatementBuilder.BuildStatement();
            }
        }

        private IEnumerable<CreateIndexStatementCollection> GetCreateIndexStatements()
        {
            foreach ( var entityMapping in _model.EntityMappings )
            {
                var indexStatementBuilder = new CreateIndexStatementBuilder( _model, entityMapping );

                yield return indexStatementBuilder.BuildStatement();
            }
        }

        #endregion
    }
}
