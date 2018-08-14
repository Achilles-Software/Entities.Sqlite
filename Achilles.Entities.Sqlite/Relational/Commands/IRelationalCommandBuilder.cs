﻿#region Namespaces

using Achilles.Entities.Modelling.Mapping;

#endregion

namespace Achilles.Entities.Relational.Statements
{
    public interface IRelationalCommandBuilder
    {
        IRelationalCommand Build<TEntity>( RelationalStatementKind statementKind, TEntity entity, IEntityMapping model );
    }
}
