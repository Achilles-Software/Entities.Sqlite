#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Relational.SqlStatements
{
    public interface ISqlStatement
    {
        /// <summary>
        /// Gets the statement SQL text.
        /// </summary>
        /// <returns>The statement SQL text</returns>
        string GetText();
    }
}
