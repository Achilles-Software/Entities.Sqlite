#region Namespaces

using Achilles.Entities.Relational.Statements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Update
{
    internal class ColumnSetStatementCollection : Collection<ISqlStatement>, ISqlStatementCollection
    {
        private const string ColumnSetStatementSeperator = ", ";

        public ColumnSetStatementCollection() { }

        public ColumnSetStatementCollection( IEnumerable<ISqlStatement> statements )
        {
            foreach ( var statement in statements )
            {
                Add( statement );
            }
        }

        public string GetText()
        {
            return String.Join( ColumnSetStatementSeperator, this.Select( c => c.GetText() ) );
        }
    }
}
