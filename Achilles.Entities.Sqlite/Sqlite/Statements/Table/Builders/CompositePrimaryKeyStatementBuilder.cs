#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
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
