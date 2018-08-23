using Achilles.Entities;
using Achilles.Entities.Linq;
using System.Collections.Generic;

namespace Entities.Sqlite.Tests.Data
{
    public class Customer
    {
        public Customer()
        {
            Orders = new EntityCollection<Order>();
        }

        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id;

        /// <summary>
        /// Gets or sets the customer first name property.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the customer last name property.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the customer orders.
        /// </summary>
        /// <remarks>
        /// This is a one to many relationship. One customer may have many orders.
        /// </remarks>
        public EntityCollection<Order> Orders { get; set; }
    }
}
