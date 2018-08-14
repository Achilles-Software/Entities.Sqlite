#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class CompositePrimaryKeyStatementBuilder : ISqlStatementBuilder<CompositePrimaryKeyStatement>
    {
        private readonly IEnumerable<IColumnMapping> keyMembers;

        public CompositePrimaryKeyStatementBuilder( IEnumerable<IColumnMapping> keyMembers )
        {
            this.keyMembers = keyMembers;
        }

        public CompositePrimaryKeyStatement BuildStatement()
        {
            return new CompositePrimaryKeyStatement( keyMembers.Select( km => km.MemberName ) );
        }
    }
}
