﻿#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Modelling
{
    public interface IEntityModel
    {
        /// <summary>
        /// Gets all the entity mappings in the model.
        /// </summary>
        /// <returns>A read only collection of <see cref="IEntityMapping"/>.</returns>
        IReadOnlyCollection<IEntityMapping> EntityMappings { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityMapping"></param>
        /// <returns></returns>
        bool TryGetEntityMapping( Type entityType, out IEntityMapping entityMapping );

        /// <summary>
        /// Gets an entity mapping by entity type.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <returns>The <see cref="IEntityMapping"/> for the entity type.</returns>
        IEntityMapping GetEntityMapping( Type entityType );

        /// <summary>
        /// Gets an entity mapping by the generic TEntity type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IEntityMapping GetEntityMapping<TEntity>() where TEntity : class;
    }
}
