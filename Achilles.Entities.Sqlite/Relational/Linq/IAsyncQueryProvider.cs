#region Namespaces

using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    public interface IAsyncQueryProvider : IQueryProvider
    {
        DataContext Context { get; }

        Task<TResult> ExecuteAsync<TResult>( Expression expression, CancellationToken cancellationToken );
    }
}
