#region Namespaces

using Achilles.Entities.Relational.Query.Linq;
using Achilles.Entities.Mapping;
using Achilles.Entities.Storage;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Storage
{
    // TJT: Remove before publishing

    //public class SqliteRelationalCommand : IRelationalCommand
    //{
    //    #region Fields

    //    private SqliteRelationalConnection _connection;
    //    private DbCommand _command;

    //    #endregion

    //    #region Constructor(s)

    //    internal SqliteRelationalCommand( SqliteRelationalConnection connection )
    //    {
    //        _connection = connection;
    //        _command = _connection.DbConnection.CreateCommand();
    //    }

    //    #endregion

    //    #region Public Properties

    //    public string CommandText { get; set; } = String.Empty;

    //    public DbCommand DbCommand => _command;

    //    #endregion

    //    #region Public Methods

    //    public int ExecuteNonQuery()
    //    {
    //        //if ( _conn.Trace )
    //        //{
    //        //    _conn.Tracer?.Invoke( "Executing: " + this );
    //        //}
    //        return _command.ExecuteNonQuery();
            
    //    }
        
    //    protected virtual void OnInstanceCreated( object obj )
    //    {
    //        // Can be overridden.
    //    }

    //    public IEnumerable<dynamic> Query( string sql, Parameters parameters, IDbTransaction transaction )
    //    {
    //        using ( var reader = _command.ExecuteReader() )
    //        {
    //            var names = Enumerable.Range( 0, reader.FieldCount ).Select( reader.GetName ).ToList();

    //            foreach ( IDataRecord record in reader )
    //            {
    //                var expando = new ExpandoObject() as IDictionary<string, object>;

    //                foreach ( var name in names )
    //                    expando[ name ] = record[ name ];

    //                yield return expando;
    //            }
    //        }
    //    }

    //    //public List<T> ExecuteQuery<T>()
    //    //{
    //    //    return ExecuteDeferredQuery<T>( _connection.GetMapping( typeof( T ) ) ).ToList();
    //    //}

    //    //public IEnumerable<T> ExecuteDeferredQuery<T>( DbTableMapping map )
    //    //{
    //    //    if ( _connection.Trace )
    //    //    {
    //    //        _connection.Tracer?.Invoke( "Executing Query: " + this );
    //    //    }

    //    //    return null;

    //    //}

    //    //public IEnumerable<T> ExecuteQuery<T>( DbTableMapping map )
    //    //{
    //    //    if ( _connection.Trace )
    //    //    {
    //    //        _connection.Tracer?.Invoke( "Executing Query: " + this );
    //    //    }

    //    //    return null;
    //    //}

    //    public override string ToString()
    //    {
    //        //var parts = new string[ 1 + _bindings.Count ];
    //        //parts[ 0 ] = CommandText;
    //        //var i = 1;
    //        //foreach ( var b in _bindings )
    //        //{
    //        //    parts[ i ] = string.Format( "  {0}: {1}", i - 1, b.Value );
    //        //    i++;
    //        //}

    //        var parts = String.Empty;
    //        return string.Join( Environment.NewLine, parts );
    //    }

    //    #endregion
    //}
}
