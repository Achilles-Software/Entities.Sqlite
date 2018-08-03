#region Namespaces

using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Delete
{
    internal class DeleteStatement : ISqlStatement
    {
        #region Fields

        private const string Template = "DELETE FROM {table-name} WHERE {pk-name} = @{pk-parameter-name};";

        #endregion

        #region Constructor(s)

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{table-name}", TableName );
            sb.Replace( "{pk-name}", PkName );
            sb.Replace( "{pk-parameter-name}", PkParameterName );

            return sb.ToString();
        }

        #endregion

        #region Public Properties

        public string TableName { get; set; }
        public string PkName { get; set; }
        public string PkParameterName { get; set; }
        public SqlParameterCollection Parameters { get; set; } = new SqlParameterCollection();

        #endregion
    }
}
