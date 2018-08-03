#region Namespaces

using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Statements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Update
{
    internal class UpdateStatement : ISqlStatement, ISqlParameters
    {
        #region Fields

        private const string Template = "UPDATE {table-name} SET {column-set} WHERE( {pk-column} = @{pk-parameter} );";

        #endregion

        #region Public Properties

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{table-name}", TableName );
            sb.Replace( "{column-set}", ColumnSetStatementCollection.GetText() );
            sb.Replace( "{pk-column}", PkColumn );
            sb.Replace( "{pk-parameter}", PkParameter );

            return sb.ToString();
        }

        public string TableName { get; set; }

        public ISqlStatementCollection ColumnSetStatementCollection { get; set; }

        public string PkColumn { get; set; }

        public string PkParameter { get; set; }

        public SqlParameterCollection Parameters { get; set; }
        
        #endregion
    }
}
