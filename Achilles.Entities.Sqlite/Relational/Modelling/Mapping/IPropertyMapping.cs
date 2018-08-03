#region Namespaces

using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping
{
    public interface IPropertyMapping
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> metadata.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the property name obtained from the <see cref="PropertyInfo"/>.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets the property <see cref="Type"/> obtained from the <see cref="PropertyInfo"/>.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        string ColumnType { get; set; }

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
