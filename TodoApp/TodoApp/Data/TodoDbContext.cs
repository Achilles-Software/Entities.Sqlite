using Achilles.Entities;
using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;

namespace TodoApp
{
    public class TodoDbContext: DbContext
    {
        private static TodoDbContext _dbContext;

        public DbTable<TodoItem> TodoItems { get; set; }

        public TodoDbContext( DbContextOptions options ) : base( options )
        {
            TodoItems = new DbTable<TodoItem>( this );
        }

        public static TodoDbContext Create( string connectionString )
        {
            if ( _dbContext == null )
            {
                var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

                _dbContext = new TodoDbContext( options );
            }

            return _dbContext;
        }

        /// <summary>
        /// Overrided for context options configuration.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );

            // Add context options configuration here...
        }

        /// <summary>
        /// Override for configuring entity mapping.
        /// </summary>
        /// <param name="config"></param>
        protected override void OnModelMapping( MappingConfiguration config )
        {
            config.Entity<TodoItem>( builder =>
            {
                builder.ToTable( "Todos" );

                builder.Property( p => p.Id )
                    .IsKey();

                builder.Index( p => p.Name ).Name( "IX_TodoItem_Name" ).IsUnique();

            } );

            base.OnModelMapping( config );
        }
    }

}
