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
    public class QueriesAsyncTest
    {
        [Fact]
        public async void Queries_AnyAsync_Works()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var hasPlum = await context.Products.AnyAsync( p => p.Name == "Plum" );
                var hasPears = await context.Products.AnyAsync( p => p.Name == "Pear" );

                Assert.True( hasPlum );
                Assert.False( hasPears );
            }
        }

        [Fact]
        public async void Queries_CountAsync_Works()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var productCount = await context.Products.CountAsync();

                Assert.Equal( 2, productCount );
            }
        }

        [Fact]
        public async void Queries_FirstAsync_Works()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var plum = await context.Products.FirstAsync( p => p.Name == "Plum" );

                Assert.True( plum.Name == "Plum" );
            }
        }

        [Fact]
        public async void Queries_SumAsync_Works()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var productCost = await context.Products.SumAsync( p => (decimal)p.Price );

                Assert.True( productCost.Equals( 8.0 ) );
            }
        }
    }
}
