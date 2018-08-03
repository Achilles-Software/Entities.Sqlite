#region Namespaces

using Achilles.Entities.Relational.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Achilles.Entities.Sqlite.Configuration
{
    public class SqliteOptions : RelationalOptions
    {
        #region Private Fields

        private bool _enforceForeignKeys = true;

        #endregion

        #region Constructor(s)

        public SqliteOptions()
        {
        }

        protected SqliteOptions( SqliteOptions copyFrom )
            : base( copyFrom )
        {
        }

        #endregion

        public virtual bool EnforceForeignKeys => _enforceForeignKeys;

        public virtual SqliteOptions WithEnforceForeignKeys( bool enforceForeignKeys )
        {
            _enforceForeignKeys = enforceForeignKeys;

            return this;
        }

        public override RelationalOptions Clone()
            => new SqliteOptions( this );

        internal override void AddServices( IServiceCollection services )
        {
            services.AddSqliteServices();
        }
    }
}
