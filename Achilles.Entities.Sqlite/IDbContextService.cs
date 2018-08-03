namespace Achilles.Entities
{
    public interface IDbContextService
    {
        // TODO: Use IDbContext
        DbContext Instance { get; }

        IDbContextService Initialize( DbContext context );
    }
}
