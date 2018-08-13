#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    public interface IIndexMapping
    {
        PropertyInfo PropertyInfo { get; }

        string PropertyName { get; }

        string Name { get; set; }

        bool IsUnique { get; set; }

        int Order { get; set; }
    }
}
