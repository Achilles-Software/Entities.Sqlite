#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Storage;
using System.Data;

#endregion

namespace Achilles.Entities.Relational.Statements
{
    public class RelationalCommandExecutor : IRelationalCommandExecutor
    {
        private readonly DbContext _dbContext;
        private readonly IRelationalCommandBuilder _commandBuilder;
        private readonly IDbTransaction _transaction;

        public RelationalCommandExecutor( DbContext context, IRelationalCommandBuilder commandBuilder )
        {
            _dbContext = context;
            _commandBuilder = commandBuilder;

            // TODO: Support Transactions.
            //_transaction = transaction;
        }

        private IRelationalConnection Connection => _dbContext.Database.Connection;

        public int ExecuteNonQuery<TEntity>( RelationalStatementKind statementKind, TEntity entity, IEntityMapping entityMapping ) where TEntity : class
        {
            var command = _commandBuilder.Build( statementKind, entity, entityMapping );
            
            return Connection.ExecuteNonQuery( command.Sql, command.Parameters.ToDictionary() );
        }
    }
}
