#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Materializing
{
    internal class EntityTypeMap
    {
        /// <summary>
        /// Creates a new <see cref="EntityTypeMap"/>.
        /// </summary>
        /// <param name="type">Entity Type to materialize.</param>
        /// <param name="identifiers">The <paramref name="type"/>s identifiers.</param>
        /// <param name="propertiesAndFields">The <paramref name="type"/>s properties and fields.</param>
        public EntityTypeMap( Type type, IEnumerable<string> identifiers, Dictionary<string, MemberInfo> propertiesAndFields )
        {
            Type = type;
            Identifiers = identifiers;
            PropertiesAndFieldsInfo = propertiesAndFields;
        }

        /// <summary>
        /// Type for this TypeMap
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// List of identifiers
        /// </summary>
        public IEnumerable<string> Identifiers;

        /// <summary>
        /// Property/field names and their corresponding PropertyInfo/FieldInfo objects
        /// </summary>
        //public Dictionary<string, object> PropertiesAndFieldsInfo;
        public Dictionary<string, MemberInfo> PropertiesAndFieldsInfo;
    }
}
