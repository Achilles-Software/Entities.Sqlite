using Microsoft.Extensions.DependencyInjection;

namespace Achilles.Entities.Configuration
{
    public interface IDbContextOptions
    {
        void AddServices( IServiceCollection services );
    }
}
