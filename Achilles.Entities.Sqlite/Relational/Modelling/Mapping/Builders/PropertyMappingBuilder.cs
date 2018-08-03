#region Namespaces

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping.Builders
{
    public class PropertyMappingBuilder : IPropertyMappingBuilder
    {
        public PropertyMappingBuilder( PropertyInfo propertyInfo )
        {
            Property = propertyInfo;
            PropertyMapping = CreatePropertyMap( propertyInfo );
        }

        public PropertyInfo Property { get; }

        public IPropertyMapping PropertyMapping { get; }

        protected virtual IPropertyMapping CreatePropertyMap( PropertyInfo propertyInfo ) => new PropertyMapping( propertyInfo );

        public IPropertyMappingBuilder ToColumn( string columnName )
        {
            PropertyMapping.ColumnName = columnName;

            return this;
        }

        public IPropertyMappingBuilder IsKey()
        {
            PropertyMapping.IsKey = true;

            return this;
        }

        public IPropertyMappingBuilder IsRequired()
        {
            PropertyMapping.IsRequired = true;

            return this;
        }

        public IPropertyMappingBuilder IsUnique()
        {
            PropertyMapping.IsUnique = true;

            return this;
        }

        public void Ignore()
        {
            PropertyMapping.Ignore = true;
        }

        public IPropertyMapping Build()
        {
            return PropertyMapping;
        }
    }
}
