#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Relational
{
    /// <summary>
    /// Represents an SQL command parameter.
    /// </summary>
    public class SqlParameter
    {
        /// <summary>
        /// Constructs a new SQL parameter with a name, value pair.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public SqlParameter( string name, object value )
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the name of the parameter
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        public object Value { get; private set; }
    }
}
