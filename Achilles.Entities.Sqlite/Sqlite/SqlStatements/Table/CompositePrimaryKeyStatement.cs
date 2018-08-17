#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class CompositePrimaryKeyStatement : Collection<string>, ISqlStatement
    {
        private const string Template = "PRIMARY KEY({primary-keys})";
        private const string PrimaryKeyColumnNameSeperator = ", ";

        public CompositePrimaryKeyStatement( IEnumerable<string> keyMembers )
        {
            foreach ( var keyMember in keyMembers )
            {
                Add( keyMember );
            }
        }

        public string GetText()
        {
            string primaryKeys = String.Join( PrimaryKeyColumnNameSeperator, this );

            return Template.Replace( "{primary-keys}", primaryKeys );
        }
    }
}
