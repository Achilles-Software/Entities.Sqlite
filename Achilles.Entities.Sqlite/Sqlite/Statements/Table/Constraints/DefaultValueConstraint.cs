#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class DefaultValueConstraint : ISqlStatement
    {
        private const string Template = "DEFAULT ({defaultValue})";

        public string DefaultValue { get; set; }

        public string GetText()
        {
            var sb = new StringBuilder(Template);

            sb.Replace("{defaultValue}", DefaultValue);

            return sb.ToString().Trim();
        }
    }
}
