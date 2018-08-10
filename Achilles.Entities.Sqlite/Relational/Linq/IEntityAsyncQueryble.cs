#region Namespaces

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    public interface IEntityAsyncQueryable
    {
        //Task<IReadOnlyList<TResult>> ToListAsync<TResult>( CancellationToken token );

        Task<bool> AnyAsync( CancellationToken token );

        Task<int> CountAsync( CancellationToken token );
        
        Task<TResult> FirstAsync<TResult>( CancellationToken token );
        Task<TResult> FirstOrDefaultAsync<TResult>( CancellationToken token );

        Task<TResult> SingleAsync<TResult>( CancellationToken token );
        Task<TResult> SingleOrDefaultAsync<TResult>( CancellationToken token );

        Task<TResult> SumAsync<TResult>( CancellationToken token );
        Task<TResult> MinAsync<TResult>( CancellationToken token );
        Task<TResult> MaxAsync<TResult>( CancellationToken token );
        Task<double> AverageAsync( CancellationToken token );
    }
}
