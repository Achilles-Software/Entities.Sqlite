using Achilles.Entities;

namespace Entities.Sqlite.Tests.Data
{
    public class Product
    {
        public Product ()
        {
            Supplier = new EntityReference<Supplier>();
            Parts = new EntityCollection<Part>();
        }

        /// <summary>
        /// Gets or sets the order id. Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the SupplierId. Foreign key. Required.
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the supplier entity. HasOne, 1-1 relationship.
        /// </summary>
        public EntityReference<Supplier> Supplier { get; set; }

        /// <summary>
        /// Gets or sets the set of product parts. HasMany, 1-many relationship.
        /// </summary>
        public EntityCollection<Part> Parts { get; set; }
    }
}
