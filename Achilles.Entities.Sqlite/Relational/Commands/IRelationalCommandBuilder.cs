#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;

#endregion

namespace Achilles.Entities.Relational.Commands
{
    public interface IRelationalCommandBuilder
    {
        IRelationalCommand Build<TEntity>( SqlStatementKind statementKind, IEntityModel model, TEntity entity, IEntityMapping mapping );
    }
}
