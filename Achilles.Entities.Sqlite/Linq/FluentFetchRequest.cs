#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Remotion.Linq;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq
{
    public class FluentFetchRequest<TQueried, TFetch> : QueryableBase<TQueried>
    {
        public FluentFetchRequest( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }
    }
}
