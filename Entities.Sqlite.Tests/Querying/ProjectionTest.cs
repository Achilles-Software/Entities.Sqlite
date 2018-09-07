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
    public class ProjectionsTest
    {
        public class ProductDto
        {
            public string ProductDescription { get; set; }
        }

        public class JoinedProductSuppliers
        {
            public Product Product { get; set; }
            public Supplier Supplier { get; set; }
        }

        [Fact]
        public void Queries_Projections_CanProjectToJoinObject()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var q = from p in context.Products
                        join s in context.Suppliers on p.Id equals s.Id
                        select new JoinedProductSuppliers
                        {
                            Product = p,
                            Supplier = s
                        };

                var result = q.ToList();

                Assert.Equal( 2, result.Count() );

                Assert.Equal( "Banana", result[ 0 ].Product.Name );
                Assert.Equal( "Bananas-R-Us", result[ 0 ].Supplier.Name );

                Assert.Equal( "Plum", result[ 1 ].Product.Name );
                Assert.Equal( "Plums-R-Us", result[ 1 ].Supplier.Name );
            }
        }

        [Fact]
        public void Queries_Projections_CanProjectToDTO()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var q = from p in context.Products
                        select new ProductDto
                        {
                            ProductDescription = p.Name
                        };

                var result = q.ToList();

                Assert.Equal( 2, result.Count() );
                Assert.Equal( "Banana", result[ 0 ].ProductDescription );
                Assert.Equal( "Plum", result[ 1 ].ProductDescription );
            }
        }
    }
}
