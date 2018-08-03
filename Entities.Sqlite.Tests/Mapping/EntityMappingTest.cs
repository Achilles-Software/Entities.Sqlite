#region Namespaces

using Achilles.Entities.Mapping;
using Entities.Sqlite.Tests.Data;
using System.Linq;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests
{
    public class EntityMappingTest
    {
        [Fact]
        public void Mapping_has_valid_configuration_entity_mapping()
        {
            string connectionString = "Data Source=test.db";

            using ( var context = TestDbContext.Create( connectionString ) )
            {
                // TJT: Specific test for datasource
                //var result = context.Database.DbConnection.DataSource;
                //Assert.Equal( "test.db", Path.GetFileName( result ) );

                Assert.Single( context.Model.EntityMappings );

                var entityMapping = context.Model.EntityMappings.Values.First();
                Assert.IsType<EntityMapping<Product>>( entityMapping );

                var productEntityMapping = (EntityMapping<Product>)entityMapping;
                Assert.Equal( "Products", productEntityMapping.TableName );

                Assert.Equal( 4, productEntityMapping.PropertyMappings.Count );

                var idPropertyMapping = productEntityMapping.PropertyMappings.First( p => p.PropertyName == "Id" );
                Assert.IsType<PropertyMapping>( idPropertyMapping );
                Assert.True( idPropertyMapping.IsKey );

                var pricePropertyMapping = productEntityMapping.PropertyMappings.First( p => p.PropertyName == "Price" );
                Assert.IsType<PropertyMapping>( pricePropertyMapping );
                Assert.False( pricePropertyMapping.IsKey );
                Assert.True( pricePropertyMapping.IsRequired );

                var salutationPropertyMapping = productEntityMapping.PropertyMappings.First( p => p.PropertyName == "Salutation" );
                Assert.IsType<PropertyMapping>( salutationPropertyMapping );
                Assert.False( salutationPropertyMapping.IsKey );
                Assert.Equal( "Salutation", salutationPropertyMapping.ColumnName );
                Assert.True( salutationPropertyMapping.Ignore );

                var nameIndexMapping = productEntityMapping.IndexMappings.First( p => p.PropertyName == "Name" );
                Assert.IsType<IndexMapping>( nameIndexMapping );
                Assert.Equal( "IX__Products_Name", nameIndexMapping.Name );
                Assert.True( nameIndexMapping.IsUnique );
            }
        }

        [Fact]
        public void Mapping_can_get_set_property_value()
        {
            string connectionString = "Data Source=test.db";

            using ( var context = TestDbContext.Create( connectionString ) )
            {
                Assert.Single( context.Model.EntityMappings );

                var entityMapping = context.Model.EntityMappings.Values.First();
                Assert.IsType<EntityMapping<Product>>( entityMapping );

                var productEntityMapping = (EntityMapping<Product>)entityMapping;
                Assert.Equal( "Products", productEntityMapping.TableName );

                Product testProduct = new Product()
                {
                    Id = 6,
                    Name = "Banana",
                    Price = 6.45,
                    Salutation = "Mr. Banana Man"
                };

                var name = productEntityMapping.GetPropertyValue( testProduct, "Name" );

                Assert.Equal( "Banana", name );

                var price = 5.46;
                productEntityMapping.SetPropertyValue( testProduct, "Price", price );
                var newPrice = productEntityMapping.GetPropertyValue( testProduct, "Price" );

                Assert.Equal( 5.46, newPrice );
            }
        }

    }
}
