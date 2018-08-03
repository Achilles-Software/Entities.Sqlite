#region Namespaces

using Achilles.Entities.Relational.Query.Linq;
using Achilles.Entities.Mapping;
using Achilles.Entities.Storage;
using Remotion.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

#endregion

namespace Achilles.Entities.Relational.Query
{
    public class DbTableQueryExecuter : IQueryExecutor
    {
        private readonly DbContext _context;
        private readonly IRelationalConnection _connection;
        private readonly IDbTransaction _transaction;

        public DbTableQueryExecuter( DbContext context )
        {
            _context = context;
            _connection = context.Database.Connection;
            //_transaction = transaction;
        }

        public T ExecuteScalar<T>( QueryModel queryModel )
        {
            SqlQueryModelVisitor visitor = new SqlQueryModelVisitor( _context );

            visitor.VisitQueryModel( queryModel );
            string sql = visitor.GetSql();

            var result = _connection.ExecuteScalar( sql, visitor.Parameters.ToDictionary() );//, _transaction );

            return (T)Convert.ChangeType( result, typeof( T ) ); 
        }

        public T ExecuteSingle<T>( QueryModel queryModel, bool returnDefaultWhenEmpty )
        {
            var result = ExecuteCollection<T>( queryModel );

            return returnDefaultWhenEmpty ? result.SingleOrDefault() : result.Single();
        }

        public IEnumerable<T> ExecuteCollection<T>( QueryModel queryModel )
        {
            SqlQueryModelVisitor visitor = new SqlQueryModelVisitor( _context );
            visitor.VisitQueryModel( queryModel );
            string sql = visitor.GetSql();

            var queryResult = _connection.ExecuteReader( sql, visitor.Parameters, _transaction );

            return AutoMapper.MapDynamic<T>( queryResult );
        }
    }
}
