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
    public class DbTableQueryable<TResult> : QueryableBase<TResult> //, IAsyncEnumerable<TResult>, IListSource
    {
        //public DbTableQueryable( IAsyncQueryProvider provider )
        //    : base( provider )
        //{
        //}

        public DbTableQueryable( DbContext context )
            : base( QueryParser.CreateDefault(), new DbTableQueryExecuter( context ) )
        {
        }

        public DbTableQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }

        #region IListSource Implementation

        public bool ContainsListCollection => false;

        public IList GetList()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IAsyncEnumerable Implementation

        // Required for async linq

        //IAsyncEnumerator<TResult> IAsyncEnumerable<TResult>.GetEnumerator()
        //{
        //    var temp = ((IAsyncQueryProvider)Provider).ExecuteAsync<TResult>( Expression ).GetEnumerator();

        //    return temp;
        //}

        //IAsyncEnumerator<TResult> IAsyncEnumerable<TResult>.GetEnumerator()
        //{
        //    var temp = GetEnumerator() as IAsyncEnumerator<TResult>;
        //    return temp;
        //}
        
        #endregion
    }
}
