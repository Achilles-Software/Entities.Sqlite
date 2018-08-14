#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Storage;
using System.Data;

#endregion

namespace Achilles.Entities.Relational.Commands
{
    public class RelationalCommandExecutor : IRelationalCommandExecutor
    {
        private readonly DataContext _dbContext;
        private readonly IRelationalCommandBuilder _commandBuilder;
        private readonly IDbTransaction _transaction;

        public RelationalCommandExecutor( DataContext context, IRelationalCommandBuilder commandBuilder )
        {
            _dbContext = context;
            _commandBuilder = commandBuilder;

            // TODO: Support Transactions.
            //_transaction = transaction;
        }

        private IRelationalConnection Connection => _dbContext.Database.Connection;

        public int ExecuteNonQuery<TEntity>( SqlStatementKind statementKind, TEntity entity, IEntityMapping entityMapping ) where TEntity : class
        {
            var command = _commandBuilder.Build( statementKind, entity, entityMapping );
            
            return Connection.ExecuteNonQuery( command.Sql, command.Parameters.ToDictionary() );
        }
    }
}
