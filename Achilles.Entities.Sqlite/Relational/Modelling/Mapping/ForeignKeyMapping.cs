#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    public class ForeignKeyMapping : IForeignKeyMapping
    {
        #region  Constructor(s)

        public ForeignKeyMapping( PropertyInfo propertyInfo )
        {
            PropertyInfo = propertyInfo;
        }

        #endregion

        #region Public Properties

        public PropertyInfo PropertyInfo { get; }

        public string PropertyName => PropertyInfo.Name;

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        #endregion
    }
}
