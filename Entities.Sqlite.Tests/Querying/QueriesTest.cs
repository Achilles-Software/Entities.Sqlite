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
    public class QueriesTest
    {
        [Fact]
        public void Queries_Simple_CanReadList()
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

                Assert.Single( products );
                Assert.Equal( "Banana", products[ 0 ].Name );
            }
        }

        [Fact]
        public async Task Querying_Simple_CanReadListAsync()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var products = await context.Products.ToListAsync();

                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }
    }
}
