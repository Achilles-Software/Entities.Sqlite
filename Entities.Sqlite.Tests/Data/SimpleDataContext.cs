#region Namespaces

using Achilles.Entities;
using Achilles.Entities.Configuration;
using Achilles.Entities.Modelling;
using Achilles.Entities.Sqlite.Configuration;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    public class SimpleDataContext : DataContext
    {
        public EntitySet<Address> Addresses { get; set; }
        
        public SimpleDataContext( DataContextOptions options ) : base( options )
        {
            Addresses = new EntitySet<Address>( this );
        }

         /// <summary>
        /// Override for configuring entity mapping.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected internal override void OnModelBuilding( EntityModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Address>( entity =>
            {
                //entity.ToTable( "Addresses" );

                //entity.Column( p => p.AddressId )
                //    .IsKey();
            } );

            base.OnModelBuilding( modelBuilder );
        }
    }
}
