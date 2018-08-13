﻿#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    public interface IEntityMapping
    {
        List<IColumnMapping> ColumnMappings { get; }

        List<IIndexMapping> IndexMappings { get; }

        List<IForeignKeyMapping> ForeignKeyMappings { get; }

        Type EntityType { get; }

        string SchemaName { get; set; }

        string TableName { get; set; }

        /// <summary>
        /// Gets a value indicating this entity mapping is case-sensitive.
        /// </summary>
        bool IsCaseSensitive { get; set; }

        object GetPropertyValue<T>( T entity, string propertyName ) where T : class ;

        void SetPropertyValue<T>( T entity, string propertyName, object value ) where T : class;

        void Compile();
    }
}
