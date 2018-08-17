#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    public interface IIndexMappingBuilder
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        //PropertyInfo IndexInfo { get; }

        /// <summary>
        /// Sets the index name.
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns>A <see cref="IIndexMappingBuilder"/> instance.</returns>
        IIndexMappingBuilder Name( string indexName );

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A <see cref="IIndexMappingBuilder"/> instance.</returns>
        IIndexMappingBuilder IsUnique();

        /// <summary>
        /// Builds and returns the <see cref="IndexMapping"/>.
        /// </summary>
        /// <returns>A <see cref="IIndexMapping"/> instance.</returns>
        //IIndexMapping Build();
    }
}
