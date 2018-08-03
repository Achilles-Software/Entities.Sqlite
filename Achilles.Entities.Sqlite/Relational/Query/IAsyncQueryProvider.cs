using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Achilles.Entities.Relational.Query
{
    public interface IAsyncQueryProvider : IQueryProvider
    {
        IAsyncEnumerable<TResult> ExecuteAsync<TResult>( Expression expression );

        Task<TResult> ExecuteAsync<TResult>( Expression expression, CancellationToken cancellationToken );
    }
}
