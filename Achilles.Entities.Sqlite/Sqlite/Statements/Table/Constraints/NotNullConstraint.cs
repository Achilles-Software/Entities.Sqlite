#region Namespaces

using Achilles.Entities.Relational.Statements;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class NotNullConstraint : ISqlStatement
    {
        public string GetText()
        {
            return "NOT NULL";
        }
    }
}