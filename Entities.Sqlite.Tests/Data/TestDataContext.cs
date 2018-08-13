#region Namespaces

using Achilles.Entities;
using Achilles.Entities.Sqlite.Configuration;
using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Modelling;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    public class TestDataContext : DataContext
    {
        private static TestDataContext _dataContext;

        public EntitySet<Product> Products { get; set; }

        public TestDataContext( DataContextOptions options ) : base( options )
        {
            Products = new EntitySet<Product>( this );
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
        /// <param name="config"></param>
        protected internal override void OnModelBuilding( RelationalModelBuilder config )
        {
            //config.Entity<Customer>( builder =>
            //{
            //    builder.Column( p => p.Orders );

            //    builder.Column( p => p.Id )
            //        .IsKey();

            //    builder.HasMany( p => p.Orders )
            //        .WithForeignKey<Order>( o => o.CustomerId );
            //} );

            //config.Entity<Order>( builder =>
            //{
            //    builder.Column( p => p.Id )
            //        .IsKey();

            //    builder.HasOne( p => p.Customer )
            //        .WithForeignKey( p => p.CustomerId )
            //        .Name( "CustomerFK" )
            //        .IsRequired();

            //    builder.HasMany( p => p.Products )
            //        .WithForeignKey<Product>( p => p.OrderId );
            //} );

            config.Entity<Product>( builder =>
            {
                builder.ToTable( "Products" );

                builder.Column( p => p.Price )
                    .IsRequired();

                builder.Column( p => p.Salutation )
                    .Ignore();

                builder.Column( p => p.Id )
                    .IsKey();

                builder.HasIndex( p => p.Name ).Name( "IX_Products_Name" ).IsUnique();

                //builder.HasOne( p => p.Supplier )
                //    .WithForeignKey( p => p.SupplierId )
                //    .IsRequired();
            } );

            base.OnModelBuilding( config );
        }
    }
}
