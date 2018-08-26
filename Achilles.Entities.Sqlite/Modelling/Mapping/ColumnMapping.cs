#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// Implements the <see cref="IColumnMapping"/> interface.
    /// </summary>
    public class ColumnMapping : IColumnMapping
    {
        #region  Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="ColumnMapping "/> from the provided <see cref="ColumnInfo"/> parameter.
        /// </summary>
        /// <param name="memberInfo">A <see cref="System.Reflection.MemberInfo"/> instance.</param>
        public ColumnMapping( MemberInfo memberInfo )
        {
            ColumnInfo = memberInfo;
            
            // TODO: TypeMapping Must be a Sqlite specific DI service

            var typeMapping = SqliteTypeMapping.FindMapping( PropertyType );

            if ( typeMapping != null )
            {
                // The property is a simple scalar type. It is mapped to a database column.
                ColumnName = ColumnInfo.Name;
                ColumnDataType = typeMapping.MappingTypeName;
            }
            else
            {
                throw new NotSupportedException( "The property cannot be mapped. Name: " + PropertyName + ", Type: " + PropertyType.ToString() );
            }
        }

        #endregion

        #region Public Properties

        //public Action<TEntity> Setter => CreateSetter<TEntity>(); 

        /// <inheritdoc />
        public MemberInfo ColumnInfo { get; }

        /// <inheritdoc />
        public Type PropertyType => ColumnInfo.GetPropertyType();
        
        /// <inheritdoc />
        public string PropertyName => ColumnInfo.Name;

        /// <inheritdoc />
        public string ColumnName { get; set; }

        /// <inheritdoc />
        public string ColumnDataType { get; set; }

        /// <inheritdoc />
        public bool IsKey { get; set; } = false;

        /// <inheritdoc />
        public bool IsRequired { get; set; } = false;

        /// <inheritdoc />
        public bool Ignore { get; set; } = false;

        /// <inheritdoc />
        public int? MaxLength { get; set; }

        /// <inheritdoc />
        public string DefaultValue { get; set; } = string.Empty;

        /// <inheritdoc />
        public bool IsUnique { get; set; } = false;

        #endregion
    }
}
