#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class ForeignKeyStatement : ISqlStatement
    {
        #region Private Fields

        private const string Template = "FOREIGN KEY ({foreign-key}) REFERENCES {parent-table}({parent-key})";
        private const string CascadeDeleteStatement = "ON DELETE CASCADE";

        #endregion

        #region Public Properties

        // TODO: Use IEnumerable<string> for composite key support
        public string ForeignKey { get; set; } 
        public string ParentTable { get; set; }
        // TODO: Use IEnumerable<string> for composite key support
        public string ParentKey { get; set; }
        public bool CascadeDelete { get; set; }

        #endregion

        #region Public Methods

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{foreign-key}", string.Join( ", ", ForeignKey ) );
            sb.Replace( "{parent-table}", ParentTable );
            sb.Replace( "{parent-key}", string.Join( ", ", ParentKey ) );

            if ( CascadeDelete )
            {
                sb.Append( " " + CascadeDeleteStatement );
            }

            return sb.ToString();
        }

        #endregion
    }
}
