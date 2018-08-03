#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class PrimaryKeyConstraint : ISqlStatement
    {
        private const string Template = "PRIMARY KEY {autoincrement}";

        public bool Autoincrement { get; set; }

        public string GetText()
        {
            var sb = new StringBuilder(Template);

            sb.Replace("{autoincrement}", Autoincrement ? "AUTOINCREMENT" : string.Empty);

            return sb.ToString().Trim();
        }
    }
}