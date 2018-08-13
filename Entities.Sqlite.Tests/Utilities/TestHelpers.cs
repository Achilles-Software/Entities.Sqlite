#region Namespaces

using Achilles.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Entities.Sqlite.Tests.Utilities
{
    public abstract class TestHelpers
    {
        public abstract IServiceCollection AddProviderServices( IServiceCollection services );

        protected abstract void UseProviderOptions( DataContextOptionsBuilder optionsBuilder );
    }
}
