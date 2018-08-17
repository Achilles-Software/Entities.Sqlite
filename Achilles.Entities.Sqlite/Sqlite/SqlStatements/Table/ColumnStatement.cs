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
    internal class ColumnStatement : ISqlStatement
    {
        private const string Template = "[{column-name}] {type-name} {column-constraint}";

        public string GetText()
        {
            var sb = new StringBuilder( Template );

            sb.Replace( "{column-name}", ColumnName );
            sb.Replace( "{type-name}", TypeName );
            sb.Replace( "{column-constraint}", ColumnConstraints.CommandText() );

            return sb.ToString().Trim();
        }

        public string ColumnName { get; set; }

        public string TypeName { get; set; }

        public ColumnConstraintCollection ColumnConstraints { get; set; }
    }
}
