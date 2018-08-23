#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// Relation mapping data.
    /// </summary>
    public class RelationshipMapping : IRelationshipMapping
    {
        /// <inheritdoc/>
        public MemberInfo RelationshipProperty { get; internal set; }

        /// <inheritdoc/>
        public IForeignKeyMapping ForeignKeyMapping { get; internal set; }

        /// <inheritdoc/>
        public bool IsMany { get; internal set; }
    }
}
