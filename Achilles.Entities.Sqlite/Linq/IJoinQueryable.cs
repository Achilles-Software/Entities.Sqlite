#region Namespaces

using System.Linq;

#endregion

namespace Achilles.Entities.Linq
{
    public interface IJoinQueryable<out TEntity, out TProperty> : IQueryable<TEntity>
    {
    }
}
