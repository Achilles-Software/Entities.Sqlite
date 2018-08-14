#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
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
