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

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    public class IndexMappingBuilder : IIndexMappingBuilder
    {
        public IndexMappingBuilder( MemberInfo indexInfo )
        {
            IndexInfo = indexInfo;
            IndexMapping = CreateIndexMapping( indexInfo );
        }

        public MemberInfo IndexInfo { get; }

        public IIndexMapping IndexMapping { get; }

        public IIndexMappingBuilder Name( string indexName )
        {
            IndexMapping.Name = indexName;

            return this;
        }

        public IIndexMappingBuilder IsUnique()
        {
            IndexMapping.IsUnique = true;

            return this;
        }

        public IIndexMapping Build() => IndexMapping;

        protected virtual IIndexMapping CreateIndexMapping( MemberInfo indexInfo ) => new IndexMapping( indexInfo );
    }
}
