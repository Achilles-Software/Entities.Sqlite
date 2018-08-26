#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces


#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// The Index mapping properties.
    /// </summary>
    public interface IIndexMapping
    {
        /// <summary>
        /// Gets the <see cref="IndexInfo"/> on the entity that the index is on
        /// </summary>
        // IndexInfo { get; }

        /// <summary>
        /// Gets the property name of this index mapping.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets or sets the index name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the index is unique.
        /// </summary>
        bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets the index order.
        /// </summary>
        int Order { get; set; }
    }
}
