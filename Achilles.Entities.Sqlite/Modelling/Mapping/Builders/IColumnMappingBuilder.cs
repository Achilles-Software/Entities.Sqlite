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

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// A fluent builder api for creating and configuring a <see cref="ColumnMapping"/>.
    /// </summary>
    public interface IColumnMappingBuilder
    {
        /// <summary>
        /// Gets a reference to the <see cref="IColumnMapping"/> being built.
        /// </summary>
        IColumnMapping ColumnMapping { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        MemberInfo Column { get; }

        /// <summary>
        /// Specifies the column name for this property.
        /// </summary>
        /// <param name="columnName">The name of the column in the underlying data store.</param>
        /// <returns>This <see cref="IColumnMappingBuilder"/> instance.</returns>
        IColumnMappingBuilder ToColumn( string columnName );

        /// <summary>
        /// Sets the column to be a key.
        /// </summary>
        /// <returns>This <see cref="IColumnMappingBuilder"/> instance.</returns>
        IColumnMappingBuilder IsKey();

        /// <summary>
        /// Sets the column ...
        /// </summary>
        /// <returns>This <see cref="IColumnMappingBuilder"/> instance.</returns>
        IColumnMappingBuilder IsRequired();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>This <see cref="IColumnMappingBuilder"/> instance.</returns>
        IColumnMappingBuilder IsUnique();

        /// <summary>
        /// 
        /// </summary>
        void Ignore();
        
        /// <summary>
        /// Builds the <see cref="IColumnMapping"/> instance from this builder instance.
        /// </summary>
        /// <returns>The <see cref="IColumnMapping"/>.</returns>
        IColumnMapping Build();
    }
}
