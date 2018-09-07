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

namespace Entities.Sqlite.Tests.Querying
{
    public class DeferredLoadingTest
    {
        [Fact]
        public void Querying_LazyLoading_CanLoadEntityReference()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                Assert.Equal( 2, context.Products.Count() );

                var query = from p in context.Products
                            where p.Name == "Banana"
                            select p;

                var products = query.ToList<Product>();

                Assert.False( products[ 0 ].Supplier.IsLoaded );
                Assert.Equal( "Bananas-R-Us", products[ 0 ].Supplier.Value.Name );
                Assert.True( products[ 0 ].Supplier.IsLoaded );
            }
        }

        [Fact]
        public void Querying_LazyLoading_CanLoadEntityCollection()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                Assert.Equal( 2, context.Products.Count() );

                var query = from p in context.Products
                            where p.Name == "Banana"
                            select p;

                var products = query.ToList<Product>();

                Assert.False( products[ 0 ].Parts.IsLoaded );

                var count = products[ 0 ].Parts.Count;
                Assert.Equal( 3, count );

                var parts = products[ 0 ].Parts.ToList();

                Assert.Equal( "Bolt", parts[ 0 ].Name );
                Assert.Equal( "Wrench", parts[ 1 ].Name );
                Assert.Equal( "Hammer", parts[ 2 ].Name );

                Assert.True( products[ 0 ].Parts.IsLoaded );
            }
        }
    }
}
