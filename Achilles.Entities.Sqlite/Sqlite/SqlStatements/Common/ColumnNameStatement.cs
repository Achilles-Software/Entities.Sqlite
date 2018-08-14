#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Common
{
    internal class ColumnNameStatement : ISqlStatement
    {
        private const string Template = "[{column-name}]";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{column-name}", ColumnName );
          
            return sb.ToString().Trim();
        }

        public string ColumnName { get; set; }
    }
}
