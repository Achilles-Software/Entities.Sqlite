#region Namespaces

using Achilles.Entities.Relational.SqlStatements;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class NotNullConstraint : ISqlStatement
    {
        public string GetText()
        {
            return "NOT NULL";
        }
    }
}