#region Namespaces

using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Common
{
    internal class PkColumnParameterStatement
    {
        private const string Template = "{pk-column-name} = @{pk-parameter-name}";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{pk-column-name}", PkColumnName );
            sb.Replace( "{pk-parameter-name}", PkParameterName );

            return sb.ToString().Trim();
        }

        public string PkColumnName { get; set; }
        public string PkParameterName { get; set; }
    }
}
