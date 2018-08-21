#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Storage;
using Remotion.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using Achilles.Entities.Mapping;
using Achilles.Entities.Querying;

#endregion

namespace Achilles.Entities.Linq
{
    public class EntityQueryExecutor : IQueryExecutor
    {
        #region Private Fields

        private readonly DataContext _context;
        private readonly IRelationalConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly EntityMaterializer _materializer;

        #endregion

        #region Constructor(s)

        public EntityQueryExecutor( DataContext context )
        {
            _context = context;
            _connection = context.Database.Connection;
            //_transaction = transaction;
            _materializer = new EntityMaterializer( context.Model );
        }

        #endregion

        #region IQueryExecutor API

        public T ExecuteScalar<T>( QueryModel queryModel )
        {
            SqliteQueryModelVisitor visitor = new SqliteQueryModelVisitor( _context );

            visitor.VisitQueryModel( queryModel );
            
            var result = _connection.ExecuteScalar( visitor.GetSql(), visitor.Parameters.ToDictionary() );//, _transaction );

            return (T)Convert.ChangeType( result, typeof( T ) ); 
        }

        public T ExecuteSingle<T>( QueryModel queryModel, bool returnDefaultWhenEmpty )
        {
            var result = ExecuteCollection<T>( queryModel );

            return returnDefaultWhenEmpty ? result.SingleOrDefault() : result.Single();
        }

        public IEnumerable<T> ExecuteCollection<T>( QueryModel queryModel )
        {
            SqliteQueryModelVisitor visitor = new SqliteQueryModelVisitor( _context );
            visitor.VisitQueryModel( queryModel );

            var queryResult = _connection.ExecuteReader( visitor.GetSql(), visitor.Parameters, _transaction );

            return _materializer.Materialize<T>( queryResult );
        }

        #endregion
    }
}
