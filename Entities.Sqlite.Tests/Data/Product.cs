using System.Collections.Generic;

namespace Entities.Sqlite.Tests.Data
{
    public class Product
    {
        //public Product()
        //{
        //    //Orders = new HashSet<Order>();
        //}

        /// <summary>
        /// Gets or sets the order id. Primary key.
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Salutation { get; set; }

        //public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the SupplierId. Foreign key. Required.
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the supplier entity. 1-1 relationship.
        /// </summary>
        public virtual Supplier Supplier { get; set; }
    }
}
