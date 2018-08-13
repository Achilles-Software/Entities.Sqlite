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
    public interface IForeignKeyMapping
    {
        PropertyInfo PropertyInfo { get; }

        string PropertyName { get; }

        string Name { get; set; }

        bool IsRequired { get; set; }
    }
}
