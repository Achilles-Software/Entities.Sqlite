using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Relational.Commands
{
    public class RelationalCommand : IRelationalCommand
    {
        public RelationalCommand( string sql, SqlParameterCollection parameters )
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; }

        public SqlParameterCollection Parameters { get; }
    }
}
