#region Copyright Notice

// Copyright (c) by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping.Builders
{
    public interface IPropertyMappingBuilder
    {
        /// <summary>
        /// Gets a reference to the <see cref="IPropertyMapping"/> being built.
        /// </summary>
        IPropertyMapping PropertyMapping { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        PropertyInfo Property { get; }

        /// <summary>
        /// Specifies the column name for this property.
        /// </summary>
        /// <param name="columnName">The name of the column in the underlying data store.</param>
        /// <returns>This <see cref="IPropertyMappingBuilder"/> instance.</returns>
        IPropertyMappingBuilder ToColumn( string columnName );

        IPropertyMappingBuilder IsKey();

        IPropertyMappingBuilder IsRequired();

        IPropertyMappingBuilder IsUnique();

        void Ignore();
        
        /// <summary>
        /// Builds the <see cref="IPropertyMapping"/> instance from this builder instance.
        /// </summary>
        /// <returns>A <see cref="IPropertyMapping"/> instance.</returns>
        IPropertyMapping Build();
    }
}
