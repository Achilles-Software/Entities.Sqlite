#region Namespaces

using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    /// <summary>
    /// Represents an entity column mapping.
    /// </summary>
    public interface IColumnMapping
    {
        /// <summary>
        /// Gets the <see cref="MemberInfo"/> metadata.
        /// </summary>
        MemberInfo MemberInfo { get; }

        bool IsPropertyMember { get; }

        /// <summary>
        /// Gets the backing method name obtained from the <see cref="MemberInfo"/>.
        /// </summary>
        string MemberName { get; }

        /// <summary>
        /// Gets the method <see cref="Type"/> obtained from the <see cref="MemberInfo"/>.
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// Gets the flag indicating that the property is a mapped to a database column.
        /// </summary>
        bool IsColumn { get; }
        
        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        string ColumnType { get; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        string ColumnName { get; set; }

        string DefaultValue { get; set; }

        int? MaxLength { get; set; }

        bool IsRequired { get; set; }

        bool Ignore { get; set; }

        bool IsKey { get; set; }

        bool IsUnique { get; set; }
    }
}
