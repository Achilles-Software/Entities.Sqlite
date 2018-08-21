namespace Entities.Sqlite.Tests.Data
{
    public class Part
    {
        /// <summary>
        /// Gets or sets the primary key by convention.
        /// </summary>
        public int PartId { get; set; }

        public string Name { get; set; }

        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets the product key. This is a foreign key.
        /// </summary>
        public int ProductKey { get; set; }
    }
}
