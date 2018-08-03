#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Properties;
using Achilles.Entities.Relational.Modelling;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Storage;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities
{
    public class DbContext : IDisposable
    {
        #region Fields

        private ServiceCollection _services;
        private IServiceProvider _serviceProvider = null;
        private IRelationalDatabase _database = null;
        private IRelationalModel _model = null;
        private IDbContextService _contextService = null;
        private IRelationalCommandExecutor _commandExecutor = null;
        private IRelationalCommandBuilder _commandBuilder = null;

        private bool _isConfiguring = false;
        private bool _isModelBuilding = false;

        #endregion

        #region Constructor(s)

        protected DbContext()
            : this( new DbContextOptions<DbContext>() )
        {
        }

        public DbContext( DbContextOptions options )
        {
            Options = options ?? throw new ArgumentNullException( nameof( options ) );
        }

        #endregion

        #region Private Properties

        private IServiceProvider ServiceProvider
        {
            get
            {
                if ( _serviceProvider != null )
                {
                    return _serviceProvider;
                }

                if ( _isConfiguring )
                {
                    throw new InvalidOperationException( Resources.OnConfiguringCalledRecursively );
                }

                try
                {
                    _isConfiguring = true;

                    var optionsBuilder = new DbContextOptionsBuilder( Options );

                    OnConfiguring( optionsBuilder );

                    var options = optionsBuilder.Options;

                    InitializeServices( options );
                }
                finally
                {
                    _isConfiguring = false;
                }

                return _serviceProvider;
            }
        }

        private IRelationalCommandExecutor CommandExecutor
        {
            get
            {
                if ( _commandExecutor != null )
                {
                    return _commandExecutor;
                }

                _commandExecutor = ServiceProvider.GetRequiredService<IRelationalCommandExecutor>();

                return _commandExecutor;
            }
        }

        private IRelationalCommandBuilder CommandBuilder
        {
            get
            {
                if ( _commandBuilder != null )
                {
                    return _commandBuilder;
                }

                _commandBuilder = ServiceProvider.GetRequiredService<IRelationalCommandBuilder>();

                return _commandBuilder;
            }
        }

        #endregion

        #region Public Properties

        public DbContextOptions Options { get; }

        public IRelationalModel Model
        {
            get
            {
                if ( _model != null )
                {
                    return _model;
                }

                if ( _isModelBuilding )
                {
                    throw new InvalidOperationException( Resources.OnConfiguringCalledRecursively );
                }

                try
                {
                    _isModelBuilding = true;
                    var modelBuilder = ServiceProvider.GetService<IRelationalModelBuilder>();

                    _model = modelBuilder.Build( this );

                    return _model;
                }
                finally
                {
                    _isModelBuilding = false;
                }
            }
        }

        public IRelationalDatabase Database
        {
            get
            {
                return _database ?? (_database = ServiceProvider.GetService<IRelationalDatabase>());
            }
        }

        #endregion

        #region Public CRUD Methods

        public void Add<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.EntityMappings.GetMapping( typeof( TEntity ) );

            var insertCommand = CommandBuilder.Build( RelationalStatementKind.Insert, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( insertCommand.Sql, insertCommand.Parameters.ToDictionary() );

            var key = entityMapping.PropertyMappings.Where( p => p.IsKey ).FirstOrDefault();

            if ( key != null )
            {
               // TJT: Sqlite specific
                var rowId = Database.Connection.LastInsertRowId();

                entityMapping.SetPropertyValue( entity, key.PropertyName, rowId );
            }
        }

        public void Update<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.EntityMappings.GetMapping( typeof( TEntity ) );

            var updateCommand = CommandBuilder.Build( RelationalStatementKind.Update, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( updateCommand.Sql, updateCommand.Parameters.ToDictionary() );
        }

        public void Delete<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.EntityMappings.GetMapping( typeof( TEntity ) );

            var deleteCommand = CommandBuilder.Build( RelationalStatementKind.Delete, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( deleteCommand.Sql, deleteCommand.Parameters.ToDictionary() );
        }
        
        #endregion

        #region Public Methods

        //public IAsyncQueryProvider QueryProvider { get; }

        internal IEnumerable<T> QueryInternal<T>( string sql, object parameters ) where T : class
        {
            Action<IDbCommand, object> paramReader = ( command, obj ) =>
            {
                var properties = obj.GetType()
                    .GetProperties()
                    .Select( property => new { property.Name, Value = property.GetValue( obj, null ) } )
                    .ToList();

                foreach ( var propertyMap in properties )
                {
                    IDbDataParameter dbparam = command.CreateParameter();
                    dbparam.ParameterName = propertyMap.Name;
                    dbparam.Value = propertyMap.Value;
                    command.Parameters.Add( dbparam );
                }
            };

            using ( IDbCommand command = SetupStoredCommand( null, sql, parameters != null ? paramReader : null, parameters, null ) )
            {
                //Connection.OpenIfNot();

                using ( IDataReader reader = command.ExecuteReader() )
                {
                    // Materializer<T> mapper = new Materializer<T>( reader );
                    while ( reader.Read() )
                    {
                        yield return null;// (T)mapper.Materialize( reader );
                    }
                }
            }
        }

        private IDbCommand SetupStoredCommand( IDbTransaction transaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout )
        {
            return SetupCommand( transaction, sql, paramReader, obj, commandTimeout, CommandType.StoredProcedure );
        }

        private IDbCommand SetupCommand( IDbTransaction transaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType )
        {
            IDbCommand command = Database.Connection.DbConnection.CreateCommand();

            if ( transaction != null )
            {
                command.Transaction = transaction;
            }
            if ( commandTimeout.HasValue )
            {
                command.CommandTimeout = commandTimeout.Value;
            }
            if ( commandType.HasValue )
            {
                command.CommandType = commandType.Value;
            }
            command.CommandText = sql;
            if ( paramReader != null )
            {
                paramReader( command, obj );
            }

            return command;
        }

        #endregion

        #region Configuration 

        protected internal virtual void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
        }

        protected internal virtual void OnModelMapping( MappingConfiguration config )
        {
        }

        #endregion

        #region Private Methods

        private void InitializeServices( DbContextOptions options )
        {
            // Each DbContext has it's own set of services provider.
            _services = new ServiceCollection();

            // Add the services required for the specific relational options.
            options.AddServices( _services );

            _serviceProvider = _services.BuildServiceProvider();

            // Initialize the DbContextService for this context
            _contextService = _serviceProvider.GetRequiredService<IDbContextService>();
            _contextService.Initialize( this );
        }
        #endregion

        #region Dispose Pattern

        public void Dispose()
        {
            Dispose( true );
        }

        protected void Dispose( bool disposing )
        {
            if ( disposing )
            {
                Trace.WriteLine( "DbContext disposing" );

                if ( Database.Connection != null )
                {
                    Database.Connection.Close();
                }

                _serviceProvider = null;

                Trace.WriteLine( "DbContext disposing finished." );
            }
        }

        #endregion
    }
}
