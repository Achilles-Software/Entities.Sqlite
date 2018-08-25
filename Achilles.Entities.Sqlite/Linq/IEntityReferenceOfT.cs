#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Linq
{
    public interface IEntityReference<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the entity reference by lazy loading.
        /// </summary>
        TEntity Value { get; }
    }
}
