#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Storage;
using Remotion.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using Achilles.Entities.Mapping;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    public class EntityQueryExecutor : IQueryExecutor
    {
        #region Private Fields

        private readonly DataContext _context;
        private readonly IRelationalConnection _connection;
        private readonly IDbTransaction _transaction;

        #endregion

        #region Constructor(s)

        public EntityQueryExecutor( DataContext context )
        {
            _context = context;
            _connection = context.Database.Connection;
            //_transaction = transaction;
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
            string sql = visitor.GetSql();

            var queryResult = _connection.ExecuteReader( sql, visitor.Parameters, _transaction );

            return AutoMapper.MapDynamic<T>( queryResult );
        }

        #endregion
    }
}
