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
    public class EagerLoadingTest
    {
        [Fact]
        public void EntityReference_EagerLoading_CanLoadEntity()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var query = from p in context.Products
                            join s in context.Suppliers on p.SupplierId equals s.Id into psgroup
                            from s in psgroup.DefaultIfEmpty()
                            select new { Product = p, Supplier = s };

                var result = query.ToList();

                //var result = query.ToList()
                //        .GroupBy( key => key.Email, element => element.Name )
                //        .Select( g => new EmailDto { Subject = g.Key.Subject, Tags = g.Select( t => new TagDto { Name = t } ).ToList() }
            }
        }


        [Fact]
        public void EntityCollection_EagerLoading_CanLoadEntityCollection()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var query = from p in context.Products
                            join s in context.Suppliers on p.SupplierId equals s.Id into ps
                            from s in ps.DefaultIfEmpty()
                            select new { Product = p, ProductName = p == null ? "(No products)" : p.Name };

                var products = query.ToList();
            }
        }

        public class JoinedPost
        {
            public Product Product { get; set; }
            public Supplier Supplier { get; set; }
        }

        [Fact]
        public void EntityReference_EagerLoading_CanProjectToJoinEntity()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var q = from p in context.Products
                        join s in context.Suppliers on p.Id equals s.Id
                        select new { Prod = p, Supp = s };

                var result = q.ToList();

                //var result = query.ToList()
                //        .GroupBy( key => key.Email, element => element.Name )
                //        .Select( g => new EmailDto { Subject = g.Key.Subject, Tags = g.Select( t => new TagDto { Name = t } ).ToList() }
            }
        }
    }
}
