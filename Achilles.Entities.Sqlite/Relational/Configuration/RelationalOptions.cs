#region Namespaces

using Achilles.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

#endregion

namespace Achilles.Entities.Relational.Configuration
{
    public abstract class RelationalOptions : DbContextOptions, IRelationalOptions
    {
        private string _connectionString;
        private DbConnection _connection;
        private int? _commandTimeout;
        
        protected RelationalOptions()
        {
        }

        protected RelationalOptions( RelationalOptions copyFrom )
        {
            if ( copyFrom == null )
            {
                throw new ArgumentNullException( nameof( copyFrom ) );
            }

            _connectionString = copyFrom._connectionString;
            _connection = copyFrom._connection;
            _commandTimeout = copyFrom._commandTimeout;
        }
    
        public virtual string ConnectionString => _connectionString;

        public virtual RelationalOptions WithConnectionString( string connectionString )
        {
            _connectionString = connectionString;

            return this;
        }

        public virtual DbConnection Connection => _connection;

        public virtual RelationalOptions WithConnection( DbConnection connection )
        {
            _connection = connection;

            return this;
        }

        public virtual int? CommandTimeout => _commandTimeout;

        public virtual RelationalOptions WithCommandTimeout( int? commandTimeout )
        {
            if ( commandTimeout.HasValue && commandTimeout <= 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( commandTimeout ) );
            }

            _commandTimeout = commandTimeout;

            return this;
        }

        public new abstract RelationalOptions Clone();

        /// <summary>
        /// Adds the relational services for the specific relational options implementation. 
        /// </summary>
        /// <param name="services">The internal db context services collection.</param>
        //internal abstract void AddServices( IServiceCollection services ); 
    }
}
