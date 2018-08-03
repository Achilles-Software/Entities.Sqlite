#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Relational.Statements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class CompositePrimaryKeyStatementBuilder : ISqlStatementBuilder<CompositePrimaryKeyStatement>
    {
        private readonly IEnumerable<IPropertyMapping> keyMembers;

        public CompositePrimaryKeyStatementBuilder( IEnumerable<IPropertyMapping> keyMembers )
        {
            this.keyMembers = keyMembers;
        }

        public CompositePrimaryKeyStatement BuildStatement()
        {
            return new CompositePrimaryKeyStatement( keyMembers.Select( km => km.PropertyName ) );
        }
    }
}
