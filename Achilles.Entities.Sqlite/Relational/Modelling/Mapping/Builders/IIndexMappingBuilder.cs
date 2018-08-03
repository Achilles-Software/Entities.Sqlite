#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping.Builders
{
    public interface IIndexMappingBuilder
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        PropertyInfo Property { get; }

        IIndexMappingBuilder Name( string indexName );

        IIndexMappingBuilder IsUnique();

        IIndexMapping Build();
    }
}
