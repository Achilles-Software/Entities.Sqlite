using System;
using Microsoft.Extensions.DependencyInjection;

namespace Achilles.Entities.Configuration
{
    public abstract class DbContextOptions //: IDbContextOptions
    {
        public DbContextOptions()
        {
        }

        internal virtual void AddServices( IServiceCollection services )
        {
        }
    }
}
