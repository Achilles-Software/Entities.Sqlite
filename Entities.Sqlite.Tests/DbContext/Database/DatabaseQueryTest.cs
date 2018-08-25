#region Namespaces

using Achilles.Entities.Linq;
using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Entities.Sqlite.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests
{
    public class DatabaseQueryTest
    {
        public class JoinedProduct
        {
            public Product Product { get; set; }
            public Part Part { get; set; }
        }

        private void InitializeContext( TestDataContext context )
        {
            var createResult = context.Database.Creator.CreateIfNotExists();

            var suppliersList = new List<Supplier>
            {
                    new Supplier(){ Name = "Bananas-R-US" },
                    new Supplier(){ Name = "Plums-R-Us" }
            };

            var productsList = new List<Product>() {
                    new Product(){ Name = "Banana", Price = 4.75 },
                    new Product(){ Name = "Plum", Price = 3.25 },
                };

            foreach ( Supplier s in suppliersList )
            {
                context.Suppliers.Add( s );
            }

            var count = 0;
            foreach ( Product p in productsList )
            {
                p.SupplierId = suppliersList[ count++ ].Id; 
                context.Products.Add( p );
            }

            var productOneId = productsList[ 0 ].Id; 
            var productTwoId = productsList[ 1 ].Id;

            var partList = new List<Part>()
            {
                new Part() { Name = "Bolt", Cost = 1.95, ProductKey = productOneId },
                new Part() { Name = "Wrench", Cost = 5.95, ProductKey = productOneId },
                new Part() { Name = "Hammer", Cost = 9.99, ProductKey = productOneId },

                new Part() { Name = "Screw", Cost = 0.95, ProductKey = productTwoId },
                new Part() { Name = "Drill", Cost = 23.55, ProductKey = productTwoId },
                new Part() { Name = "Air Compressor", Cost = 99.95, ProductKey = productTwoId }
            };

            foreach ( var part in partList )
            {
                context.Parts.Add( part );
            }
        }

        [Fact]
        public void Database_ComplexQuery_CanReadListWithLazyEntityReference()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                InitializeContext( context );

                Assert.Equal( 2, context.Products.Count() );

                var query = from p in context.Products
                            where p.Name == "Banana"
                            select p;

                var products = query.ToList<Product>();

                Assert.False( products[ 0 ].Supplier.IsLoaded );
                Assert.Equal( "Bananas-R-US", products[ 0 ].Supplier.Value.Name );
                Assert.True( products[ 0 ].Supplier.IsLoaded );
            }
        }

        [Fact]
        public void Database_ComplexQuery_CanReadList()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                InitializeContext( context );

                Assert.Equal( 2, context.Products.Count() );

                var query = from p in context.Products
                            where p.Name == "Banana"
                            select p;

                var products = query.ToList<Product>();

                Assert.Single( products );
                Assert.Equal( "Banana", products[ 0 ].Name );
            }
        }

        [Fact]
        public async Task Database_ComplexQuery_CanReadListAsync()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                InitializeContext( context );

                var products = await context.Products.ToListAsync();

                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }
    }
}
