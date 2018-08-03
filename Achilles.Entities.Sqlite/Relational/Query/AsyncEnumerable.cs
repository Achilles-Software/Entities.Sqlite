#region Namespaces

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Relational.Query
{
    public struct AsyncEnumerable<TResult> : IAsyncEnumerableAccessor<TResult>
    {
        private readonly IAsyncEnumerable<TResult> _asyncEnumerable;

        /// <summary>
        ///     Creates a new instance of <see cref="AsyncEnumerable{TResult}" />
        /// </summary>
        /// <param name="asyncEnumerable">The underlying <see cref="IAsyncEnumerable{TResult}" /> instance.</param>
        public AsyncEnumerable( IAsyncEnumerable<TResult> asyncEnumerable )
        {
            _asyncEnumerable = asyncEnumerable;
        }

        IAsyncEnumerable<TResult> IAsyncEnumerableAccessor<TResult>.AsyncEnumerable => _asyncEnumerable;

        /// <summary>
        ///     Asynchronously creates a <see cref="List{T}" /> from this <see cref="AsyncEnumerable{T}" />
        ///     by enumerating it asynchronously.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        public async Task<List<TResult>> ToListAsync(
            CancellationToken cancellationToken = default )
        {
            var list = new List<TResult>();

            using ( var asyncEnumerator = _asyncEnumerable.GetEnumerator() )
            {
                while ( await asyncEnumerator.MoveNext( cancellationToken ) )
                {
                    list.Add( asyncEnumerator.Current );
                }
            }

            return list;
        }
    }
}
