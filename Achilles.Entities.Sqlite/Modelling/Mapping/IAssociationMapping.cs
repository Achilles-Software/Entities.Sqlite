#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    public interface IAssociationMapping
    {
        PropertyInfo PropertyInfo { get; }

        string Name { get; }

    }
}
