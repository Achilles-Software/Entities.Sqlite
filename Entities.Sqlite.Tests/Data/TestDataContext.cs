#region Namespaces

using Achilles.Entities;
using Achilles.Entities.Configuration;
using Achilles.Entities.Modelling;
using Achilles.Entities.Sqlite.Configuration;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    public class TestDataContext : DataContext
    {
        private static TestDataContext _dataContext;

        public EntitySet<Product> Products { get; set; }
        public EntitySet<Supplier> Suppliers { get; set; }
        public EntitySet<Part> Parts { get; set; }

        public TestDataContext( DataContextOptions options ) : base( options )
        {
            Products = new EntitySet<Product>( this );
            Suppliers = new EntitySet<Supplier>( this );
            Parts = new EntitySet<Part>( this );
        }

        public static TestDataContext Create( string connectionString )
        {
            if ( _dataContext == null )
            {
                var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;
 
                _dataContext = new TestDataContext( options );
            }

            return _dataContext;
        }

        /// <summary>
        /// Overrided for context options configuration.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected internal override void OnConfiguring( DataContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );

            // Add context options configuration here...
        }

        /// <summary>
        /// Override for configuring entity mapping.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected internal override void OnModelBuilding( EntityModelBuilder modelBuilder )
        {
            //config.Entity<Customer>( entity =>
            //{
            //    entity.Column( p => p.Orders );

            //    entity.Column( p => p.Id )
            //        .IsKey();

            //    entity.HasMany( p => p.Orders )
            //        .WithForeignKey<Order>( o => o.CustomerId );
            //} );

            //config.Entity<Order>( entity =>
            //{
            //    entity.Column( p => p.Id )
            //        .IsKey();

            //    entity.HasOne( p => p.Customer )
            //        .WithForeignKey( p => p.CustomerId )
            //        .Name( "CustomerFK" );
            //        //.IsRequired();

            //    entity.HasMany( p => p.Products )
            //        .WithForeignKey<Product>( p => p.OrderId );
            //} );

            modelBuilder.Entity<Supplier>( entity =>
            {
                entity.ToTable( "Suppliers" );

                entity.Column( p => p.Id )
                    .IsKey();

                entity.Column( p => p.Name )
                    .IsRequired();
            } );

            modelBuilder.Entity<Product>( entity =>
            {
                entity.ToTable( "Products" );

                entity.Column( p => p.Id )
                    .IsKey();

                entity.Column( p => p.Price )
                    .IsRequired();

                entity.HasIndex( p => p.Name )
                    .Name( "IX_Products_Name" )
                    .IsUnique();

                entity.HasOne( p => p.Supplier )           // The relationship property
                    .WithForeignKey( p => p.SupplierId )   // The foreign key. The Foreign Key constrain is on the Products table (Non-generice WithForeignKey).
                    .References<Supplier>( p => p.Id );    // The reference table and key 

                entity.HasMany( p => p.Parts )
                    .WithForeignKey<Part>( parts => parts.ProductKey )
                    .References<Product>( p => p.Id );
            } );

            base.OnModelBuilding( modelBuilder );
        }
    }
}
