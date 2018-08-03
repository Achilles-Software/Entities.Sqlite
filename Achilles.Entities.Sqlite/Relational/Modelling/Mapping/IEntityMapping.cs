#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Mapping
{
    public interface IEntityMapping
    {
        List<IPropertyMapping> PropertyMappings { get; }

        List<IIndexMapping> IndexMappings { get; }

        List<IAssociationMapping> AssociationMappings { get; }

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
