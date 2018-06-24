using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Query
{
    public class DbTableQueryBase
    {
        protected class Ordering
        {
            public string ColumnName { get; set; }
            public bool Ascending { get; set; }
        }
    }
}
