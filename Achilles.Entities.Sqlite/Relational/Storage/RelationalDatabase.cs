#region Namespaces

using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

#endregion

namespace Achilles.Entities.Storage
{
    public abstract class RelationalDatabase : IRelationalDatabase
    {
        #region Fields

        protected readonly IDataContextService _dbContext;

        #endregion
        
        #region Constructor(s)

        public RelationalDatabase( 
            IDataContextService dbContext, 
            IRelationalConnection connection, 
            IRelationalDatabaseCreator creator )
        {
            _dbContext = dbContext;
            Connection = connection;
            Creator = creator;
        }

        #endregion

        public IRelationalConnection Connection { get; private set; }

        public IRelationalDatabaseCreator Creator { get; private set; }

        public abstract bool Exists();

        public virtual Task<bool> ExistsAsync( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( Exists() );
        }

        public abstract void Create();

        public virtual Task CreateAsync( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            Create();

            return Task.FromResult( 0 );
        }

        public abstract bool Delete();

        public virtual Task<bool> DeleteAsync( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( Delete() );
        }

        public abstract bool HasTables();

        public virtual Task<bool> HasTablesAsync( CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( HasTables() );
        }
    }
}
