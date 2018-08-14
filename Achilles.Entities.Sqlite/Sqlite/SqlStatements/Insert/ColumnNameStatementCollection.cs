#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Insert
{
    internal class ColumnNameStatementCollection : Collection<ISqlStatement>, ISqlStatementCollection
    {
        private const string ColumnNameStatementSeperator = ", ";

        public ColumnNameStatementCollection() { }

        public ColumnNameStatementCollection( IEnumerable<ISqlStatement> statements )
        {
            foreach ( var statement in statements )
            {
                Add( statement );
            }
        }

        public string GetText()
        {
            return String.Join( ColumnNameStatementSeperator, this.Select( c => c.GetText() ) );
        }
    }
}
