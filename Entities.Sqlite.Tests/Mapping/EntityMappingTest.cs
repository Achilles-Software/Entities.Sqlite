#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Achilles.Entities.Modelling.Mapping;
using Entities.Sqlite.Tests.Data;
using System.Linq;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests.Modelling
{
    public class EntityMappingTest
    {
        [Fact]
        public void Mapping_has_valid_configuration_entity_mapping()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new SimpleDataContext( options ) )
            {
                Assert.Single( context.Model.EntityMappings );

                var entityMapping = context.Model.EntityMappings.First();
                Assert.IsType<EntityMapping<Address>>( entityMapping );

                var addressEntityMapping = (EntityMapping<Address>)entityMapping;
                Assert.Equal( "Address", addressEntityMapping.TableName );

                Assert.Equal( 4, addressEntityMapping.ColumnMappings.Count );

                var idPropertyMapping = addressEntityMapping.ColumnMappings.First( p => p.PropertyName == "AddressId" );
                Assert.IsType<ColumnMapping>( idPropertyMapping );
                Assert.True( idPropertyMapping.IsKey );

                var streetPropertyMapping = addressEntityMapping.ColumnMappings.First( p => p.PropertyName == "Street" );
                Assert.IsType<ColumnMapping>( streetPropertyMapping );
                Assert.False( streetPropertyMapping.IsKey );
                
                var cityPropertyMapping = addressEntityMapping.ColumnMappings.First( p => p.PropertyName == "City" );
                Assert.IsType<ColumnMapping>( cityPropertyMapping );
                Assert.False( cityPropertyMapping.IsKey );
                Assert.Equal( "City", cityPropertyMapping.ColumnName );
            }
        }

        [Fact]
        public void Mapping_can_get_set_property_value()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new SimpleDataContext( options ) )
            {
                Assert.Single( context.Model.EntityMappings );

                var entityMapping = context.Model.EntityMappings.First();
                Assert.IsType<EntityMapping<Address>>( entityMapping );

                var addressEntityMapping = (EntityMapping<Address>)entityMapping;

                var testAddress = new Address()
                {
                    AddressId = 1,
                    Street = "123 Banana Seat",
                    City = "Somewhere",
                    Country = "Canada"
                };

                var country = addressEntityMapping.GetColumn( testAddress, "Country" );
                Assert.Equal( "Canada", country );

                addressEntityMapping.SetColumn( testAddress, "Country", "USA" );
                var newCountry = addressEntityMapping.GetColumn( testAddress, "Country" );

                Assert.Equal( "USA", newCountry );
            }
        }

    }
}
