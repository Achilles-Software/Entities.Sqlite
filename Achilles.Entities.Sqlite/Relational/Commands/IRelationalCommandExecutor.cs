#region Namespaces

using Achilles.Entities.Modelling.Mapping;

#endregion

namespace Achilles.Entities.Relational.Statements
{
    public interface IRelationalCommandExecutor
    {
        int ExecuteNonQuery<TEntity>( RelationalStatementKind statementKind, TEntity entity, IEntityMapping entityMapping ) where TEntity : class;
    }
}
