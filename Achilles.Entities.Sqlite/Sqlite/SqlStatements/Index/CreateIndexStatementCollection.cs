#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Index
{
    internal class CreateIndexStatementCollection : Collection<ISqlStatement>, ISqlStatementCollection
    {
        private const string StatementSeperator = "\r\n";

        public CreateIndexStatementCollection( IEnumerable<ISqlStatement> createIndexStatements )
        {
            foreach ( var createIndexStatement in createIndexStatements )
            {
                Add( createIndexStatement );
            }
        }

        public string GetText()
        {
            return String.Join( StatementSeperator, this.Select( e => e.GetText() ) );
        }
    }
}
