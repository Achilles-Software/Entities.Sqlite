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
    public class IndexMappingBuilder : IIndexMappingBuilder
    {
        public IndexMappingBuilder( PropertyInfo propertyInfo )
        {
            Property = propertyInfo;
            Index = CreateIndexMapping( propertyInfo );
        }

        public PropertyInfo Property { get; }

        public IIndexMapping Index { get; }

        public IIndexMappingBuilder Name( string indexName )
        {
            Index.Name = indexName;

            return this;
        }

        public IIndexMappingBuilder IsUnique()
        {
            Index.IsUnique = true;

            return this;
        }

        public IIndexMapping Build() => Index;

        protected virtual IIndexMapping CreateIndexMapping( PropertyInfo propertyInfo ) => new IndexMapping( propertyInfo );
    }
}
