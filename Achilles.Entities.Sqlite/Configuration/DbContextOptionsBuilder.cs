using Microsoft.Extensions.DependencyInjection;
using System;

namespace Achilles.Entities.Configuration
{
    public class DbContextOptionsBuilder //: IDbContextOptionsBuilder
    {
        #region Constructor(s)

        public DbContextOptionsBuilder()
            :this( new DbContextOptions<DbContext>() )
        {
        }

        public DbContextOptionsBuilder( DbContextOptions options )
        {
            Options = options ?? throw new System.ArgumentNullException( nameof( options ) );
        }

        #endregion

        public DbContextOptions Options { get; internal set; }

        internal virtual void AddServices( IServiceCollection services )
        { }
    }
}
