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

namespace Achilles.Entities.Modelling.Mapping
{
    public interface IRelationshipMapping
    {
        /// <summary>
        /// Gets the property for this relationship mapping.
        /// </summary>
        MemberInfo RelationshipProperty { get; }

        /// <summary>
        /// Gets the <see cref="ForeignKeyMapping"/> for this relationship mapping.
        /// </summary>
        IForeignKeyMapping ForeignKeyMapping { get; }

        /// <summary>
        /// Gets a flag indicating the type of relationship. True indicates that the relationship is 1-Many. If False then the relationship is 1-One.
        /// </summary>
        bool IsMany { get; }
    }
}
