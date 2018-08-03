#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class UniqueConstraint : ISqlStatement
    {
        private const string Template = "UNIQUE {conflict-clause}";

        public ConflictAction ConflictAction { get; set; }

        public string GetText()
        {
            var sb = new StringBuilder(Template);

            sb.Replace("{conflict-clause}", ConflictAction != ConflictAction.None ? "ON CONFLICT " + ConflictAction.ToString().ToUpperInvariant() : string.Empty);

            return sb.ToString().Trim();
        }
    }
}
