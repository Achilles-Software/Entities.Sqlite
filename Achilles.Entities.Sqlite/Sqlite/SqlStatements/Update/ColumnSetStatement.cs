#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Update
{
    internal class ColumnSetStatement : ISqlStatement
    {
        private const string Template = "{column-name} = @{column-parameter}";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{column-name}", ColumnName );
            sb.Replace( "{column-parameter}", ColumnParameter );
          
            return sb.ToString().Trim();
        }

        public string ColumnName { get; set; }
        public string ColumnParameter { get; set; }
    }
}
