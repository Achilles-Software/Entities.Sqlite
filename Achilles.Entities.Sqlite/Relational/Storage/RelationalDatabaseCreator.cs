#region Namespaces

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

#endregion

namespace Achilles.Entities.Storage
{
    public abstract class RelationalDatabaseCreator : IRelationalDatabaseCreator
    {
        #region Fields

        protected IDbContextService _dbContext;

        #endregion

        #region Constructor(s)

        protected RelationalDatabaseCreator( IDbContextService dbContext )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException( "dbContext" );
        }

        #endregion

        #region Public API Methods

        public abstract bool CreateIfNotExists();

        public virtual Task<bool> CreateIfNotExistsAsync( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( CreateIfNotExists() );
        }

        public abstract string GenerateCreateScript();

        #endregion
    }
}
