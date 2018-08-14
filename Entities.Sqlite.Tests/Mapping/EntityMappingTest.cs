#region Namespaces

using Achilles.Entities.Modelling.Mapping;
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

            using ( var context = TestDataContext.Create( connectionString ) )
            {
                // TJT: Specific test for datasource
                //var result = context.Database.DbConnection.DataSource;
                //Assert.Equal( "test.db", Path.GetFileName( result ) );

                Assert.Single( context.Model.EntityMappings );

                var entityMapping = context.Model.EntityMappings.Values.First();
                Assert.IsType<EntityMapping<Product>>( entityMapping );

                var productEntityMapping = (EntityMapping<Product>)entityMapping;
                Assert.Equal( "Products", productEntityMapping.TableName );

                Assert.Equal( 4, productEntityMapping.ColumnMappings.Count );

                var idPropertyMapping = productEntityMapping.ColumnMappings.First( p => p.MemberName == "Id" );
                Assert.IsType<ColumnMapping>( idPropertyMapping );
                Assert.True( idPropertyMapping.IsKey );

                var pricePropertyMapping = productEntityMapping.ColumnMappings.First( p => p.MemberName == "Price" );
                Assert.IsType<ColumnMapping>( pricePropertyMapping );
                Assert.False( pricePropertyMapping.IsKey );
                Assert.True( pricePropertyMapping.IsRequired );

                var salutationPropertyMapping = productEntityMapping.ColumnMappings.First( p => p.MemberName == "Salutation" );
                Assert.IsType<ColumnMapping>( salutationPropertyMapping );
                Assert.False( salutationPropertyMapping.IsKey );
                Assert.Equal( "Salutation", salutationPropertyMapping.ColumnName );
                Assert.True( salutationPropertyMapping.Ignore );

                var nameIndexMapping = productEntityMapping.IndexMappings.First( p => p.PropertyName == "Name" );
                Assert.IsType<IndexMapping>( nameIndexMapping );
                Assert.Equal( "IX_Products_Name", nameIndexMapping.Name );
                Assert.True( nameIndexMapping.IsUnique );
            }
        }

        [Fact]
        public void Mapping_can_get_set_property_value()
        {
            string connectionString = "Data Source=test.db";

            using ( var context = TestDataContext.Create( connectionString ) )
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
