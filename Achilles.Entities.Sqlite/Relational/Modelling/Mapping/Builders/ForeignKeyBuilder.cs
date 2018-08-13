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

namespace Achilles.Entities.Relational.Modelling.Mapping.Builders
{
    public class ForeignKeyBuilder : IForeignKeyBuilder
    {
        public ForeignKeyBuilder( PropertyInfo propertyInfo )
        {
            Property = propertyInfo;
            ForeignKey = CreateForeignKeyMapping( propertyInfo );
        }

        public PropertyInfo Property { get; }

        public IForeignKeyMapping ForeignKey { get; }

        public IForeignKeyBuilder Name( string foreignKeyName )
        {
            ForeignKey.Name = foreignKeyName;

            return this;
        }

        public IForeignKeyBuilder IsRequired()
        {
            ForeignKey.IsRequired = true;

            return this;
        }

        public IForeignKeyMapping Build() => ForeignKey;

        protected virtual IForeignKeyMapping CreateForeignKeyMapping( PropertyInfo propertyInfo ) => new ForeignKeyMapping( propertyInfo );

    }
}
