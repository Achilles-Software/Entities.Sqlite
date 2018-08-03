#region Namespaces

using Achilles.Entities.Relational.Statements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class ColumnConstraintCollection : Collection<ISqlStatement>
    {
        private const string ConstraintStatementSeperator = " ";

        public ColumnConstraintCollection()
            : this( new List<ISqlStatement>() )
        { }

        public ColumnConstraintCollection( IEnumerable<ISqlStatement> columnConstraints )
        {
            foreach ( var columnConstraint in columnConstraints )
            {
                Add( columnConstraint );
            }
        }

        public string CommandText()
        {
            return String.Join( ConstraintStatementSeperator, this.Select( c => c.GetText() ) );
        }
    }
}
