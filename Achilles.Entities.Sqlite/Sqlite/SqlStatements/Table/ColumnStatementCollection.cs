#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class ColumnStatementCollection : Collection<ISqlStatement>, ISqlStatementCollection
    {
        private const string ColumnStatementSeperator = ", ";

        public ColumnStatementCollection() { }

        public ColumnStatementCollection( IEnumerable<ISqlStatement> statements )
        {
            foreach ( var statement in statements )
            {
                Add( statement );
            }
        }

        public string GetText()
        {
            return String.Join( ColumnStatementSeperator, this.Select( c => c.GetText() ) );
        }
    }
}
