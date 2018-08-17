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

namespace Achilles.Entities.Modelling.Mapping
{
    public class ForeignKeyMapping : IForeignKeyMapping
    {
        #region  Constructor(s)

        public ForeignKeyMapping( MemberInfo foreignKeyProperty )
        {
            ForeignKeyProperty = foreignKeyProperty;
        }

        #endregion

        #region Public Properties

        public MemberInfo ForeignKeyProperty{ get; }

        public string PropertyName => ForeignKeyProperty.Name;

        public MemberInfo ReferenceKeyProperty { get; set; }

        public string Name { get; set; }
                
        //public string ForeignKeyColumn { get; set; }

        //public string ReferenceTable { get; set; }

        //public string ReferenceColumn { get; set; }

        public bool CascadeDelete { get; set; }

        #endregion
    }
}
