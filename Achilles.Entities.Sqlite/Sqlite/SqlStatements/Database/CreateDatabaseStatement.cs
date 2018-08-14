#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Database
{
    internal class CreateDatabaseStatement : Collection<ISqlStatement>, ISqlStatement
    {
        private const string StatementSeperator = "\r\n";

        public CreateDatabaseStatement()
        { }

        public CreateDatabaseStatement( IEnumerable<ISqlStatement> statements )
        {
            foreach ( var statement in statements )
            {
                Add( statement );
            }
        }

        public string GetText()
        {
            return String.Join( StatementSeperator, this.Select( c => c.GetText() ) );
        }
    }
}
