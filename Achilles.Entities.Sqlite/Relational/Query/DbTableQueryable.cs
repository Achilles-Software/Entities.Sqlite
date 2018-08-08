#region Namespaces

using Achilles.Entities.Storage;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Achilles.Entities.Relational.Query
{
    public class DbTableQueryable<TResult> : QueryableBase<TResult>
    {
        #region Constructor(s)

        public DbTableQueryable( DbContext context )
            : base( new DbTableQueryProvider( context, typeof( DbTableQueryable<> ), QueryParser.CreateDefault(), new DbTableQueryExecuter( context ) ) )
        {
        }

        public DbTableQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }

        #endregion
    }
}
