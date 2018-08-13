using Achilles.Entities;
using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Modelling;
using Achilles.Entities.Sqlite.Configuration;
using System.Linq;

namespace TodoApp
{
    /// <summary>
    /// The Todo app data context.
    /// </summary>
    /// <remarks>
    /// Custom data contexts derive from <see cref="DataContext"/>.
    /// </remarks>
    public class TodoDataContext: DataContext
    {
        private static TodoDataContext _dataContext;

        /// <summary>
        /// Gets or sets the Todo items.
        /// </summary>
        /// <remarks>
        /// Entity sets support the <see cref="IQueryable"/> interface.
        /// </remarks>
        public EntitySet<TodoItem> TodoItems { get; set; }

        public TodoDataContext( DataContextOptions options ) : base( options )
        {
            // Add your entity sets to the context in the data context constructor.
            TodoItems = new EntitySet<TodoItem>( this );
        }

        public static TodoDataContext Create( string connectionString )
        {
            if ( _dataContext == null )
            {
                var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

                _dataContext = new TodoDataContext( options );
            }

            return _dataContext;
        }

        /// <summary>
        /// Overrided for context options configuration.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring( DataContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );

            // Add context options configuration here...
        }

        /// <summary>
        /// Override for configuring entity mapping.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelBuilding( RelationalModelBuilder modelBuilder )
        {
            modelBuilder.Entity<TodoItem>( entity =>
            {
                entity.ToTable( "Todos" );

                entity.Column( p => p.Id )
                    .IsKey();

                entity.HasIndex( p => p.Name ).Name( "IX_TodoItem_Name" ).IsUnique();

            } );

            base.OnModelBuilding( modelBuilder );
        }
    }
}
