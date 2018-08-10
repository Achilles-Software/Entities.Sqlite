#region Namespaces

using Achilles.Entities.Sqlite.Storage;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    public class EntityQueryProvider : QueryProviderBase, IAsyncQueryProvider
    {
        private readonly Type _queryableType;

        #region Constructor(s)

        public EntityQueryProvider( DbContext context, Type queryableType, IQueryParser queryParser, IQueryExecutor executor )
          : base( queryParser, executor )
        {
            CheckQueryableType( queryableType );

            _queryableType = queryableType;

            Context = context;
        }

        #endregion

        public DbContext Context { get; }

        /// <summary>
        /// Gets the type of queryable created by this provider. This is the generic type definition of an implementation of <see cref="IQueryable{T}"/>
        /// (usually a subclass of <see cref="QueryableBase{T}"/>) with exactly one type argument.
        /// </summary>
        public Type QueryableType
        {
            get { return _queryableType; }
        }

        /// <summary>
        /// Creates a new <see cref="IQueryable"/> (of type <see cref="QueryableType"/> with <typeparamref name="T"/> as its generic argument) that
        /// represents the query defined by <paramref name="expression"/> and is able to enumerate its results.
        /// </summary>
        /// <typeparam name="T">The type of the data items returned by the query.</typeparam>
        /// <param name="expression">An expression representing the query for which a <see cref="IQueryable{T}"/> should be created.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that represents the query defined by <paramref name="expression"/>.</returns>
        public override IQueryable<T> CreateQuery<T>( Expression expression )
        {
            return (IQueryable<T>)Activator.CreateInstance( QueryableType.MakeGenericType( typeof( T ) ), this, expression );
        }

        public Task<TResult> ExecuteAsync<TResult>( Expression expression, CancellationToken cancellationToken )
        {
            var connection = Context.Database.Connection as SqliteRelationalConnection;

            var dbMethod = (Func<TResult>)Expression.Lambda( expression ).Compile();

            return connection.ExecuteAsync( dbMethod, cancellationToken );
        }

        //public Task<List<TSource>> ToListAsync<TSource>( IQueryable<TSource> source, CancellationToken cancellationToken )
        //{
        //    var connection = Context.Database.Connection as SqliteRelationalConnection;

        //    return connection.AsyncQueryA( source, cancellationToken );
        //}

        #region Private Methods

        private void CheckQueryableType( Type queryableType )
        {
            var queryableTypeInfo = queryableType.GetTypeInfo();

            if ( !queryableTypeInfo.IsGenericTypeDefinition )
            {
                var message = string.Format(
                    "Expected the generic type definition of an implementation of IQueryable<T>, but was '{0}'.",
                    queryableType );
                throw new ArgumentException( message, "queryableType" );
            }

            var genericArgumentCount = queryableTypeInfo.GenericTypeParameters.Length;

            if ( genericArgumentCount != 1 )
            {
                var message = string.Format(
                    "Expected the generic type definition of an implementation of IQueryable<T> with exactly one type argument, but found {0} arguments on '{1}.",
                    genericArgumentCount,
                    queryableType );
                throw new ArgumentException( message, "queryableType" );
            }
        }

        #endregion
    }
}
