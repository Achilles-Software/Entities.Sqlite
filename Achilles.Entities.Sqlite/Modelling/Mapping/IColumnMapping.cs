#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// Represents an entity property or field <see cref="MemberInfo"/> to database column mapping.
    /// </summary>
    public interface IColumnMapping
    {
        /// <summary>
        /// Gets the <see cref="ColumnInfo"/> metadata.
        /// </summary>
        MemberInfo ColumnInfo { get; }

        /// <summary>
        /// Gets the backing method name obtained from the <see cref="ColumnInfo"/>.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets the method <see cref="Type"/> obtained from the <see cref="ColumnInfo"/>.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets the column data type.
        /// </summary>
        string ColumnDataType { get; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets a default value to use for the column.
        /// </summary>
        string DefaultValue { get; set; }

        int? MaxLength { get; set; }

        bool IsRequired { get; set; }

        bool Ignore { get; set; }

        bool IsKey { get; set; }

        bool IsUnique { get; set; }
    }
}
