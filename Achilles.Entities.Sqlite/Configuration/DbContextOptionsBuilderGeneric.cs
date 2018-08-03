namespace Achilles.Entities.Configuration
{
    public class DbContextOptionsBuilder<TContext> : DbContextOptionsBuilder where TContext : DbContext
    {
        #region Constructor(s)

        public DbContextOptionsBuilder()
            : this( new DbContextOptions<TContext>())
        {
        }

        public DbContextOptionsBuilder( DbContextOptions<TContext> options )
            :base( options)
        {
            
        }

        #endregion

        public new virtual DbContextOptions<TContext> Options => (DbContextOptions<TContext>) base.Options;
    }
}
