#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Relational
{
    /// <summary>
    /// Represents a collection of SQL parameters. See <see cref="SqlParameter"/>.
    /// </summary>
    public class SqlParameterCollection
    {
        private List<SqlParameter> _parameters;

        #region Constructor(s)

        /// <summary>
        /// Constructs a new SqlParameterCollection instance.
        /// </summary>
        public SqlParameterCollection()
        {
            _parameters = new List<SqlParameter>();
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Adds a <see cref="SqlParameter"/> with the specified value to the collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter Add( object value )
        {
            SqlParameter parameter = new SqlParameter( string.Format( "p{0}", _parameters.Count ), value );

            _parameters.Add( parameter );

            return parameter;
        }

        /// <summary>
        /// Converts the SQL parameter collection to a name, value pair read only dictionary.
        /// </summary>
        /// <returns>The read only name, value dictionary.</returns>
        public IReadOnlyDictionary<string, object> ToDictionary()
        {
            var nameValueDictionary = _parameters.ToDictionary( p => p.Name, p => p.Value );

            return nameValueDictionary;
        }

        #endregion

        //public SqlParameter AddParameter( string name, Object value )
        //{
        //    SqlParameter parameter = new SqlParameter( name, value );

        //    _parameters.Add( parameter );

        //    return parameter;
        //}

        //public void AddParameters( IDbCommand command )
        //{
        //    foreach ( var parameter in _parameters )
        //    {
        //        var p = command.CreateParameter();

        //        p.ParameterName = parameter.Name;
        //        p.Value = parameter.Value ?? DBNull.Value;

        //        command.Parameters.Add( p );
        //    }
        //}
    }
}
