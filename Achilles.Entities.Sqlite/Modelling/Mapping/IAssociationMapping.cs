#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping
{
    public interface IAssociationMapping
    {
        PropertyInfo PropertyInfo { get; }

        string Name { get; }
    }
}
