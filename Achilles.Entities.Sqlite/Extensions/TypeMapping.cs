#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    /// <summary>
    /// Type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The list of .NET primitive types.
        /// </summary>
        public static readonly List<Type> PrimitiveTypes = new List<Type>
        {
            typeof(string),
            typeof(byte[]),
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(short),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(Guid)
        };

        /// <summary>
        /// Determines if the <see cref="Type"/> parameter is a primitive or scalar type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitive( this Type type ) => PrimitiveTypes.Contains( type );
    }
}
