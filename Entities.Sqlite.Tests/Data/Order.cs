using System.Collections.Generic;

namespace Entities.Sqlite.Tests.Data
{
    public class Order
    {
        public Order()
        {
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// The order key.
        /// </summary>
        public int Id;

        /// <summary>
        /// The customer foreign key.Required making this a 1 to 1 relationship.
        /// </summary>
        public int CustomerId;

        /// <summary>
        /// Gets or sets the customer entity for this order.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the list of products for this order. HasMany relationahip.
        /// </summary>
        public virtual HashSet<Product> Products { get; set; }
    }
}
