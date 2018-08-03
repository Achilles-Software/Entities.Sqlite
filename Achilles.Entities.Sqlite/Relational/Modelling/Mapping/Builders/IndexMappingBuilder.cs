using System.Reflection;

namespace Achilles.Entities.Mapping.Builders
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
