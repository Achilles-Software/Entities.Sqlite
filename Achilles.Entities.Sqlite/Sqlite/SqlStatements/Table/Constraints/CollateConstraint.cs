#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
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
