#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Insert
{
    internal class ColumnParameterStatement : ISqlStatement
    {
        // TJT: Escaped parameter?

        private const string Template = "@{column-parameter}";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{column-parameter}", ColumnParameter );
          
            return sb.ToString().Trim();
        }

        public string ColumnParameter { get; set; }
    }
}
