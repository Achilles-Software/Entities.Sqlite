#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class CreateTableStatement : ISqlStatement
    {
        #region Fields

        private const string Template = "CREATE TABLE {table-name} ({column-def});";

        #endregion

        #region Constructor(s)

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{table-name}", TableName );
            sb.Replace( "{column-def}", ColumnStatementCollection.GetText() );

            return sb.ToString();
        }

        #endregion

        #region Public Properties

        public string TableName { get; set; }

        public ISqlStatementCollection ColumnStatementCollection { get; set; }

        #endregion
    }

}
