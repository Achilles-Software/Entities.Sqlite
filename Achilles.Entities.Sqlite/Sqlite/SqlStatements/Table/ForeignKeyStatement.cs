#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class ForeignKeyStatement : ISqlStatement
    {
        private const string Template = "FOREIGN KEY ({foreign-key}) REFERENCES {referenced-table}({referenced-id})";
        private const string CascadeDeleteStatement = "ON DELETE CASCADE";

        public IEnumerable<string> ForeignKey { get; set; }
        public string ForeignTable { get; set; }
        public IEnumerable<string> ForeignPrimaryKey { get; set; }
        public bool CascadeDelete { get; set; }

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{foreign-key}", string.Join( ", ", ForeignKey ) );
            sb.Replace( "{referenced-table}", ForeignTable );
            sb.Replace( "{referenced-id}", string.Join( ", ", ForeignPrimaryKey ) );

            if ( CascadeDelete )
            {
                sb.Append( " " + CascadeDeleteStatement );
            }

            return sb.ToString();
        }
    }
}
