#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Insert
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
