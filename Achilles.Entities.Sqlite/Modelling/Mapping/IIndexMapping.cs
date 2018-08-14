#region Copyright Notice

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
    public interface IIndexMapping
    {
        PropertyInfo PropertyInfo { get; }

        string PropertyName { get; }

        string Name { get; set; }

        bool IsUnique { get; set; }

        int Order { get; set; }
    }
}
