#region Namespaces

using Achilles.Entities;
using Achilles.Entities.Configuration;
using Achilles.Entities.Modelling;
using Achilles.Entities.Sqlite.Configuration;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    public class IndexDataContext : DataContext
    {
        public EntitySet<Address> Addresses { get; set; }
        
        public IndexDataContext( DataContextOptions options ) : base( options )
        {
            Addresses = new EntitySet<Address>( this );
        }

        protected internal override void OnModelBuilding( EntityModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Address>( entity =>
            {
                //entity.ToTable( "Addresses" );

                //entity.Column( p => p.AddressId )
                //    .IsKey();

                entity.HasIndex( p => p.Country );
            } );

            base.OnModelBuilding( modelBuilder );
        }
    }
}
