using Achilles.Entities.Sqlite.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Sqlite.Extensions
{
    public static class SqliteRelationalConnectionExtensions
    {
        public static int ExecuteNonQuery(
            this SqliteRelationalConnection connection,
            string commandText )
        {
            using ( var command = connection.DbConnection.CreateCommand() )
            {
                command.CommandText = commandText;

                return command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(
            this SqliteRelationalConnection connection,
            string commandText )
            => (T)connection.ExecuteScalar( commandText );

        public static object ExecuteScalar( this SqliteRelationalConnection connection, string commandText )
        {
            using ( var command = connection.DbConnection.CreateCommand() )
            {
                command.CommandText = commandText;

                return command.ExecuteScalar();
            }
        }
    }
}
