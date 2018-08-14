#region Namespaces

using Achilles.Entities.Relational;
using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Insert
{
    internal class InsertStatement : ISqlStatement, ISqlParameters
    {
        #region Fields

        private const string Template = "INSERT INTO {table-name} ({column-names}) VALUES({column-parameters});";

        #endregion

        #region Public Properties

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{table-name}", TableName );
            sb.Replace( "{column-names}", ColumnNameStatementCollection.GetText() );
            sb.Replace( "{column-parameters}", ColumnParameterStatementCollection.GetText() );

            return sb.ToString();
        }

        public string TableName { get; set; }

        public ISqlStatementCollection ColumnNameStatementCollection { get; set; }

        public ISqlStatementCollection ColumnParameterStatementCollection { get; set; }

        public SqlParameterCollection Parameters { get; set; } = new SqlParameterCollection();

        #endregion
    }
}
