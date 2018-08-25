#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Linq
{
    internal interface IEntityReferenceSource
    {
        bool HasSource { get; }

        void SetSource( IEntitySet source, string referenceKey, object foreignKeyValue );
    }
}
