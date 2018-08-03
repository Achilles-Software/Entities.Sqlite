using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Sqlite.Tests.Data
{
    public class Product
    {
        public Product()
        {
            //Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Salutation { get; set; }

        //public virtual HashSet<Category> Categories {get; set;}

        //public int CategoryId { get; set; }
        //public Category Category { get; set; }
    }
}
