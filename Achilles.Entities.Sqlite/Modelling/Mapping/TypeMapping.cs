#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    public class TypeMapping
    {
        #region Constructor(s)

        public TypeMapping( Type type, string mappingTypeName )
        {
            Type = type;
            MappingTypeName = mappingTypeName; 
        }

        #endregion

        #region Public Properties

        public Type Type { get; private set; }

        public string MappingTypeName { get; private set; }

        #endregion
    }
}
