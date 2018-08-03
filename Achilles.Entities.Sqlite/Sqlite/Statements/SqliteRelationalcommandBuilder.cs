#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Sqlite.Statements.Delete;
using Achilles.Entities.Sqlite.Statements.Insert;
using Achilles.Entities.Sqlite.Statements.Table;
using Achilles.Entities.Sqlite.Statements.Update;
using System;

#endregion

namespace Achilles.Entities.Sqlite.Statements
{
    public class SqliteRelationalCommandBuilder : IRelationalCommandBuilder
    {
        public IRelationalCommand Build<TEntity>( RelationalStatementKind statementKind, TEntity entity, IEntityMapping entityMapping )
        {
            switch ( statementKind )
            {
                case RelationalStatementKind.CreateTable:

                    ISqlStatementBuilder<CreateTableStatement> statementBuilder = new CreateTableStatementBuilder( entityMapping );

                    ISqlStatement statement = statementBuilder.BuildStatement();

                    return new RelationalCommand( statement.GetText(), null );

                case RelationalStatementKind.Insert:

                    ISqlStatementBuilder<InsertStatement> insertStatementBuilder = new InsertStatementBuilder( entity, entityMapping );
                    InsertStatement insertStatement = insertStatementBuilder.BuildStatement();

                    return new RelationalCommand( insertStatement.GetText(), insertStatement.Parameters );

                case RelationalStatementKind.Update:

                    ISqlStatementBuilder<UpdateStatement> updateStatementBuilder = new UpdateStatementBuilder( entity, entityMapping );
                    UpdateStatement updateStatement = updateStatementBuilder.BuildStatement();

                    return new RelationalCommand( updateStatement.GetText(), updateStatement.Parameters );

                case RelationalStatementKind.Delete:

                    ISqlStatementBuilder<DeleteStatement> deleteStatementBuilder = new DeleteStatementBuilder( entity, entityMapping );
                    DeleteStatement deleteStatement = deleteStatementBuilder.BuildStatement();

                    return new RelationalCommand( deleteStatement.GetText(), deleteStatement.Parameters );

                default:

                    throw new NotImplementedException();
            }
        }
    }
}
