#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    /// <summary>
    /// The collation function to use for a database column.
    /// </summary>
    public enum CollationFunction
    {
        None,

        /// <summary>
        /// The same as binary, except that trailing space characters are ignored.
        /// </summary>        
        RTrim,

        /// <summary>
        /// The same as binary, except the 26 upper case characters of ASCII are folded to their lower case equivalents before
        /// the comparison is performed. Note that only ASCII characters are case folded. SQLite does not attempt to do full
        /// UTF case folding due to the size of the tables required.
        /// </summary>
        NoCase,

        /// <summary>
        /// Compares string data using memcmp(), regardless of text encoding.
        /// </summary>
        Binary,

        /// <summary>
        /// An application can register additional collating functions using the sqlite3_create_collation() interface.
        /// </summary>
        Custom
    }
}
