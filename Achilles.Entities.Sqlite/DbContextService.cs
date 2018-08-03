namespace Achilles.Entities
{
    public class DbContextService : IDbContextService
    {
        public DbContext Instance { get; private set; }

        public IDbContextService Initialize( DbContext context )
        {
            Instance = context;
            return this;
        }
    }
}
