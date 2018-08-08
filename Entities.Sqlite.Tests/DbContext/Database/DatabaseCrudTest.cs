#region Namespaces

using Achilles.Entities.Relational.Query;
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
    public class DatabaseCrudTest
    {
        private void InitializeContext( TestDbContext context )
        {
            var createResult = context.Database.Creator.CreateIfNotExists();

            var productsList = new List<Product>() {
                    new Product(){ Id = 0, Name = "Banana", Price = 4.75, Salutation = "Hello Mr. Bananas" },
                    new Product(){ Id = 0, Name = "Plum", Price = 3.25, Salutation = "Hello Mr. Plum" },
                };

            foreach ( Product p in productsList )
            {
                context.Products.Add( p );
            }
        }

        [Fact]
        public void Database_can_insert()
        {           
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                context.Products.Add( product );

                var productsCount = (from row in context.Products select row).Count();
                
                Assert.Equal( 1, productsCount );
            }
        }

        [Fact]
        public async Task Database_can_insert_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                var result = await context.Products.AddAsync( product );

                Assert.Equal( 1, result );
            }
        }

        [Fact]
        public void Database_can_read_with_pk()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;
            
            using ( var context = new TestDbContext( options ) )
            {
                InitializeContext( context );

                var product = context.Products.First( p => p.Id == 1 );
                var product2 = context.Products.First( p => p.Id == 2 );

                var products = context.Products;

                Assert.Equal( "Banana", product.Name );
                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public void Database_Query_CanReadList()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                InitializeContext( context );

                Assert.Equal( 2, context.Products.Select( p => p.Id ).Count() );
                var products = context.Products.Select( p => p ).ToList();
                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }

        [Fact]
        public async Task Database_Query_CanReadListAsync()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                InitializeContext( context );

                var products = await context.Products.Select( p => p).ToListAsync();

                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }

        [Fact]
        public void Database_can_update()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                context.Products.Add( product );
                var productsCount = context.Products.Select( p=> p.Id ).Count();

                Assert.Equal( 1, productsCount );

                product.Name = "Plum";
                context.Products.Update( product );

                var product2 = context.Products.First( p => p.Id == product.Id );

                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public async Task Database_can_update_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                await context.Products.AddAsync( product );
                var productsCount = context.Products.Select( p => p.Id ).Count();

                Assert.Equal( 1, productsCount );

                product.Name = "Plum";
                var result = await context.Products.UpdateAsync( product );

                var product2 = context.Products.First( p => p.Id == product.Id );

                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public void Database_can_delete_by_pk()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                context.Products.Add( product );
                var productsCount = context.Products.Select( p => p ).Count();

                Assert.Equal( 1, productsCount );

                context.Products.Delete( product );

                Assert.Equal( 0, context.Products.Select(p => p.Id).Count() );
           }
        }

        [Fact]
        public async Task Database_can_delete_by_pk_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    Salutation = "Hello Mr. Bananas"
                };

                context.Products.Add( product );
                var productsCount = context.Products.Select( p => p ).Count();

                Assert.Equal( 1, productsCount );

                await context.Products.DeleteAsync( product );

                Assert.Equal( 0, context.Products.Select( p => p.Id ).Count() );
            }
        }

    }
}
