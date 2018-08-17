#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    public class ColumnMappingBuilder : IColumnMappingBuilder
    {
        public ColumnMappingBuilder( MemberInfo column )
        {
            Column = column ?? throw new ArgumentNullException( nameof( column ) );

            if ( !(Column.MemberType == MemberTypes.Property || Column.MemberType == MemberTypes.Field ) )
            {
                throw new ArgumentException( nameof( column ) );
            }

            ColumnMapping = CreateColumnMapping( column );
        }

        public MemberInfo Column { get; }

        public bool IsProperty => ( Column.MemberType == MemberTypes.Property );

        public IColumnMapping ColumnMapping { get; }

        protected virtual IColumnMapping CreateColumnMapping( MemberInfo memberInfo ) => new ColumnMapping( memberInfo );

        public IColumnMappingBuilder ToColumn( string columnName )
        {
            ColumnMapping.ColumnName = columnName;

            return this;
        }

        public IColumnMappingBuilder IsKey()
        {
            ColumnMapping.IsKey = true;

            return this;
        }

        public IColumnMappingBuilder IsRequired()
        {
            ColumnMapping.IsRequired = true;

            return this;
        }

        public IColumnMappingBuilder IsUnique()
        {
            ColumnMapping.IsUnique = true;

            return this;
        }

        public void Ignore()
        {
            ColumnMapping.Ignore = true;
        }

        public IColumnMapping Build()
        {
            return ColumnMapping;
        }
    }
}
