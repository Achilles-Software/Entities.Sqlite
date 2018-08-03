#region Namespaces

using Achilles.Entities.Relational.Statements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Insert
{
    internal class ColumnParameterStatementCollection : Collection<ISqlStatement>, ISqlStatementCollection
    {
        private const string ColumnNameStatementSeperator = ", ";

        public ColumnParameterStatementCollection() { }

        public ColumnParameterStatementCollection( IEnumerable<ISqlStatement> statements )
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
