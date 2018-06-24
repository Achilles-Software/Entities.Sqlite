using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Achilles.Entities.Storage
{
    public abstract class RelationalConnectionBase : IRelationalConnection
    {
        #region Fields

        private readonly string _connectionString;
        private int _commandTimeout;

        private readonly DbConnection _connection;

        #endregion

        #region Constructor(s)

        protected RelationalConnectionBase( string connectionString, int commandTimeout = 30 )
        {
            if ( !string.IsNullOrWhiteSpace( connectionString ) )
            {
                _connectionString = connectionString;
                //_connection = DbConnection( CreateDbConnection );
            }
            else
            {
                throw new InvalidOperationException( "No connection string" );
            }

            _commandTimeout = commandTimeout;
        }

        #endregion

        #region Public/Protected Properties

        public virtual Guid ConnectionId { get; } = Guid.NewGuid();

        protected abstract DbConnection CreateDbConnection();

        public virtual string ConnectionString => _connectionString ?? DbConnection.ConnectionString;

        public virtual DbConnection DbConnection => _connection;

        public int? CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        #region Public Methods

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            if ( DbConnection.State == ConnectionState.Broken )
            {
                DbConnection.Close();
            }

            if ( DbConnection.State != ConnectionState.Open )
            {
                OpenDb();
            }
        }

        public Task<bool> OpenAsync( CancellationToken cancellationToken )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void OpenDb()
        {
            try
            {
                DbConnection.Open();
            }
            catch ( Exception e )
            {
                throw;
            }
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( true );

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
