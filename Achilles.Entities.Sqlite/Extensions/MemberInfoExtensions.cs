#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Reflection;

#endregion

namespace Achilles.Entities.Extensions
{
    public static class MemberInfoExtensions
    {
        public static Type GetPropertyType( this MemberInfo memberInfo )
            => (memberInfo as PropertyInfo)?.PropertyType ?? (memberInfo as FieldInfo)?.FieldType;
    }
}
