#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class ColumnStatement : ISqlStatement
    {
        private const string Template = "[{column-name}] {type-name} {column-constraint}";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{column-name}", ColumnName );
            sb.Replace( "{type-name}", TypeName );
            sb.Replace( "{column-constraint}", ColumnConstraints.CommandText() );

            return sb.ToString().Trim();
        }

        public string ColumnName { get; set; }

        public string TypeName { get; set; }

        public ColumnConstraintCollection ColumnConstraints { get; set; }
    }
}
