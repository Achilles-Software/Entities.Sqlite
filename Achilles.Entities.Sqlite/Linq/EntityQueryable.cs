#region Namespaces

using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Linq
{
    public class EntityQueryable<TResult> : QueryableBase<TResult> , IEntityAsyncQueryable
    {
        #region Constructor(s)

        public EntityQueryable( DataContext context, QueryParser queryParser )
            : base( new EntityQueryProvider( context, typeof( EntityQueryable<> ), queryParser, new EntityQueryExecutor( context ) ) )
        {
        }

        public EntityQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }

        #endregion

        #region Properties

        //public EntityQueryExecutor Executor => Provider.As<EntityQueryProvider>().Executor.As<EntityQueryExecutor>();

        public Task<bool> AnyAsync( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<double> AverageAsync( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountAsync( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> FirstAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> FirstOrDefaultAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> MaxAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> MinAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> SingleAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> SingleOrDefaultAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult1> SumAsync<TResult1>( CancellationToken token )
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
