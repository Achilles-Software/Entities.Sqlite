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
    public abstract class MemberAccessor
    {
        public MemberAccessor( MemberInfo memberInfo )
        {
            MemberInfo = memberInfo ?? throw new System.ArgumentNullException( nameof( memberInfo ) );
        }

        protected MemberInfo MemberInfo { get; }

        public abstract object GetValue<TMember>( TMember member );

        public abstract void SetValue<TMember>( TMember member, object value );
    }
}
