#region Namespaces

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Achilles.Entities.Configuration
{
    public class DbContextOptions<TContext> : DbContextOptions where TContext : DbContext
    {
        public DbContextOptions()
        {
        }

        #region Properties

        public DbContextOptions Options { get; internal set; }

        //////internal virtual void AddServices( IServiceCollection services )
        //////{
        //////}

        #endregion
    }
}
