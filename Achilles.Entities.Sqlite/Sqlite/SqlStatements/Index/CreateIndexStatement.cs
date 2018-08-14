#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Sqlite.SqlStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Index
{
    internal class CreateIndexStatement : ISqlStatement
    {
        private const string Template = "CREATE {unique} INDEX {index-name} ON {table-name} ({column-def});";
        private const string ColumnNameSeperator = ", ";

        public string GetText()
        {
            var stringBuilder = new StringBuilder( Template );

            stringBuilder.Replace( "{unique}", IsUnique ? "UNIQUE" : string.Empty );
            stringBuilder.Replace( "{index-name}", IndexName );
            stringBuilder.Replace( "{table-name}", TableName );

            IEnumerable<string> orderedColumnNames = Columns.OrderBy( c => c.Order ).Select( c => c.ColumnName ).Select( NameCreator.EscapeName );
            string columnDefinition = String.Join( ColumnNameSeperator, orderedColumnNames );
            stringBuilder.Replace( "{column-def}", columnDefinition );

            return stringBuilder.ToString();
        }

        public string IndexName { get; set; }

        public string TableName { get; set; }

        public ICollection<IndexColumn> Columns { get; set; }

        public bool IsUnique { get; set; }

        public class IndexColumn
        {
            public int Order { get; set; }
            public string ColumnName { get; set; }
        }
    }
}
