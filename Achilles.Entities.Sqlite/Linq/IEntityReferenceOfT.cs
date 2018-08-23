#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Linq
{
    public interface IEntityReference<TEntity>
    {
        /// <summary>
        /// Gets the wrapped entity.
        /// </summary>
        TEntity Entity { get; }
    }
}
