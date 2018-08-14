﻿#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    public class IndexMapping : IIndexMapping
    {
        #region  Constructor(s)

        public IndexMapping( PropertyInfo propertyInfo )
        {
            PropertyInfo = propertyInfo;
        }

        #endregion

        #region Public Properties

        public PropertyInfo PropertyInfo { get; }

        public string PropertyName => PropertyInfo.Name;

        public string Name { get; set; }

        public bool IsUnique { get; set; }

        public int Order { get; set; }

        #endregion
    }
}