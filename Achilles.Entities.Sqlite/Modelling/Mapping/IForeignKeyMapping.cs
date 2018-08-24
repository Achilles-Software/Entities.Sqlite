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
    /// <summary>
    /// An interface for ...
    /// </summary>
    /// <example>Sql Template = "FOREIGN KEY ({foreign-key}) REFERENCES {parent-table}({parent-key})"</example>
    public interface IForeignKeyMapping
    {
        /// <summary>
        /// Gets the foreign key <see cref="MemberInfo"/>.
        /// </summary>
        MemberInfo ForeignKeyProperty { get; }

        string PropertyName { get; }

        /// <summary>
        /// Gets the reference key <see cref="MemberInfo"/> property or field.
        /// </summary>
        MemberInfo ReferenceKeyProperty { get; set; }

        /// <summary>
        /// Gets or sets the foreign key constraint name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the flag that indictes whether ...
        /// </summary>
        bool CascadeDelete { get; set; }
    }
}
