#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.Commands;
using Achilles.Entities.Sqlite.SqlStatements.Delete;
using Achilles.Entities.Sqlite.SqlStatements.Insert;
using Achilles.Entities.Sqlite.SqlStatements.Table;
using Achilles.Entities.Sqlite.SqlStatements.Update;
using Achilles.Entities.Relational.SqlStatements;
using System;
using Achilles.Entities.Modelling;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements
{
    public class SqliteRelationalCommandBuilder : IRelationalCommandBuilder
    {
        public IRelationalCommand Build<TEntity>( SqlStatementKind statementKind, IEntityModel model, TEntity entity, IEntityMapping entityMapping )
        {
            switch ( statementKind )
            {
                case SqlStatementKind.CreateTable:

                    ISqlStatementBuilder<CreateTableStatement> statementBuilder = new CreateTableStatementBuilder( model, entityMapping );

                    ISqlStatement statement = statementBuilder.BuildStatement();

                    return new RelationalCommand( statement.GetText(), null );

                case SqlStatementKind.Insert:

                    ISqlStatementBuilder<InsertStatement> insertStatementBuilder = new InsertStatementBuilder( entity, entityMapping );
                    InsertStatement insertStatement = insertStatementBuilder.BuildStatement();

                    return new RelationalCommand( insertStatement.GetText(), insertStatement.Parameters );

                case SqlStatementKind.Update:

                    ISqlStatementBuilder<UpdateStatement> updateStatementBuilder = new UpdateStatementBuilder( entity, entityMapping );
                    UpdateStatement updateStatement = updateStatementBuilder.BuildStatement();

                    return new RelationalCommand( updateStatement.GetText(), updateStatement.Parameters );

                case SqlStatementKind.Delete:

                    ISqlStatementBuilder<DeleteStatement> deleteStatementBuilder = new DeleteStatementBuilder( entity, entityMapping );
                    DeleteStatement deleteStatement = deleteStatementBuilder.BuildStatement();

                    return new RelationalCommand( deleteStatement.GetText(), deleteStatement.Parameters );

                default:

                    throw new NotImplementedException();
            }
        }
    }
}
