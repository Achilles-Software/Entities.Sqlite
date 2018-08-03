#region Namespaces

using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Table
{
    internal class CollateConstraint : ISqlStatement
    {
        private const string Template = "COLLATE {collation-name}";

        public CollationFunction CollationFunction { get; set; }

        public string CustomCollationFunction { get; set; }

        public string GetText()
        {
            if (CollationFunction == CollationFunction.None)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(Template);

            string name = CollationFunction == CollationFunction.Custom ? CustomCollationFunction : CollationFunction.ToString().ToUpperInvariant();
            sb.Replace("{collation-name}", name);
            
            return sb.ToString().Trim();
        }
    }
}
