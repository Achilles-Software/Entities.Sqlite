#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Modelling;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Sqlite.Modelling;
using Achilles.Entities.Sqlite.Statements;
using Achilles.Entities.Sqlite.Storage;
using Achilles.Entities.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

#endregion

namespace Achilles.Entities.Sqlite.Configuration
{
    /// <summary>
    /// Dependecy injection <see cref="IServiceCollection"/> extensions.
    /// </summary>
    public static class SqliteServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Sqlite specific services and core services to the data context <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The data context ServiceCollection.</param>
        /// <returns>A <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddSqliteServices( this IServiceCollection serviceCollection )
        {
            if ( serviceCollection == null )
            {
                throw new ArgumentNullException( nameof( serviceCollection ) );
            }

            var builder = new RelationalServicesBuilder( serviceCollection )
                //.TryAddTransient<IRelationalModelBuilder, SqliteRelationalModelBuilder>()
                .TryAddSingleton<IRelationalConnection, SqliteRelationalConnection>()
                .TryAddSingleton<IRelationalDatabase, SqliteRelationalDatabase>()
                .TryAddSingleton<IRelationalDatabaseCreator, SqliteDatabaseCreator>()
                .TryAddSingleton<IRelationalCommandBuilder, SqliteRelationalCommandBuilder>();

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
