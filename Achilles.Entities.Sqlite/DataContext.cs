#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Modelling;
using Achilles.Entities.Properties;
using Achilles.Entities.Relational.Commands;
using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities
{
    /// <summary>
    /// Provides entity data services over a single database connection.  
    /// </summary>
    public class DataContext : IDisposable
    {
        #region Fields

        private ServiceCollection _services;
        private IServiceProvider _serviceProvider = null;
        private IRelationalDatabase _database = null;
        private IEntityModel _model = null;
        private IDataContextService _contextService = null;
        private IRelationalCommandBuilder _commandBuilder = null;

        private Dictionary<Type, IEntitySet> _entitySets = new Dictionary<Type, IEntitySet>();
        
        private bool _isConfiguring = false;
        private bool _isModelBuilding = false;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs an new DataContext instance with default context options. 
        /// </summary>
        protected DataContext()
            : this( new DataContextOptions<DataContext>() )
        {
        }

        /// <summary>
        /// Constructs a new DataContext with the provided <see cref="DataContextOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        public DataContext( DataContextOptions options )
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

                    var optionsBuilder = new DataContextOptionsBuilder( Options );

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

        /// <summary>
        /// Gets the data context options.
        /// </summary>
        public DataContextOptions Options { get; }

        /// <summary>
        /// Gets the relational database model.
        /// </summary>
        public IEntityModel Model
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
                    var modelBuilder = ServiceProvider.GetService<IEntityModelBuilder>();

                    _model = modelBuilder.Build( this );

                    return _model;
                }
                finally
                {
                    _isModelBuilding = false;
                }
            }
        }

        /// <summary>
        /// Gets the relational database.
        /// </summary>
        public IRelationalDatabase Database
        {
            get
            {
                return _database ?? (_database = ServiceProvider.GetService<IRelationalDatabase>());
            }
        }

        #endregion

        #region Public CRUD Methods

        /// <summary>
        /// Adds an entity of <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to add.</param>
        /// <returns>1 if the entity was added succssfully; 0 otherwise.</returns>
        public int Add<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.GetEntityMapping( typeof( TEntity ) );

            var insertCommand = CommandBuilder.Build( SqlStatementKind.Insert, Model, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( insertCommand.Sql, insertCommand.Parameters.ToDictionary() );

            var key = entityMapping.ColumnMappings.Where( p => p.IsKey ).FirstOrDefault();

            if ( key != null )
            {
               // TJT: Sqlite specific
                var rowId = Database.Connection.LastInsertRowId();

                entityMapping.SetPropertyValue( entity, key.PropertyName, rowId );
            }

            return result;
        }

        /// <summary>
        /// Asynchronously adds an entity of <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellaion token.</param>
        /// <returns></returns>
        public virtual Task<int> AddAsync<TEntity>( TEntity entity, CancellationToken cancellationToken = default ) where TEntity : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( Add( entity ) );
        }

        /// <summary>
        /// Updates an entity of <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <returns>1 if the entity was updated succssfully; 0 otherwise.</returns>
        public int Update<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.GetEntityMapping( typeof( TEntity ) );

            var updateCommand = CommandBuilder.Build( SqlStatementKind.Update, Model, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( updateCommand.Sql, updateCommand.Parameters.ToDictionary() );

            return result;
        }

        /// <summary>
        /// Updates an entity of <typeparamref name="TEntity"/> to the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>1 if the entity was updated succssfully; 0 otherwise.</returns>
        public virtual Task<int> UpdateAsync<TEntity>( TEntity entity, CancellationToken cancellationToken = default ) where TEntity : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( Update( entity ) );
        }

        /// <summary>
        /// Deletes an entity of <typeparamref name="TEntity"/> from the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>1 if the entity was deleted succssfully; 0 otherwise.</returns>
        public int Delete<TEntity>( TEntity entity ) where TEntity : class
        {
            var entityMapping = Model.GetEntityMapping( typeof( TEntity ) );

            var deleteCommand = CommandBuilder.Build( SqlStatementKind.Delete, Model, entity, entityMapping );

            var result = Database.Connection.ExecuteNonQuery( deleteCommand.Sql, deleteCommand.Parameters.ToDictionary() );

            return result;
        }

        /// <summary>
        /// Asynchronously deletes an entity of <typeparamref name="TEntity"/> from the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>1 if the entity was deleted succssfully; 0 otherwise.</returns>
        public virtual Task<int> DeleteAsync<TEntity>( TEntity entity, CancellationToken cancellationToken = default ) where TEntity : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult( Delete( entity ) );
        }

        #endregion

        #region Configuration 

        /// <summary>
        /// Override to configure the <see cref="DataContext"/>.
        /// </summary>
        /// <param name="optionsBuilder">The data context options builder.</param>
        protected internal virtual void OnConfiguring( DataContextOptionsBuilder optionsBuilder )
        {
        }

        /// <summary>
        /// Override to configure and build an entities database model.
        /// </summary>
        /// <param name="modelBuilder">A <see cref="EntityModelBuilder"/> instance.</param>
        protected internal virtual void OnModelBuilding( EntityModelBuilder modelBuilder )
        {
        }

        #endregion

        #region Private Methods

        private void InitializeServices( DataContextOptions options )
        {
            // Each DataContext has it's own set of services and provider.
            _services = new ServiceCollection();

            // Add the services required for the specific relational options.
            options.AddServices( _services );

            _serviceProvider = _services.BuildServiceProvider();

            // Initialize the DbContextService for this context
            _contextService = _serviceProvider.GetRequiredService<IDataContextService>();
            _contextService.Initialize( this );
        }

        #endregion

        #region Internal Methods

        internal void AddEntitySet<TEntity>( EntitySet<TEntity> entitySet ) where TEntity : class
        {
            _entitySets.Add( typeof(TEntity), entitySet );
        }

        internal Dictionary<Type, IEntitySet> EntitySets => _entitySets;

        #endregion

        #region Dispose Pattern

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose( true );
        }

        /// <inheritdoc />
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
