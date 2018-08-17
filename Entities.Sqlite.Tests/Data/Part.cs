using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Sqlite.Tests.Data
{
    public class Part
    {
        public int PartId { get; set; }

        public string Name { get; set; }

        public double Cost { get; set; }

        public int ProductKey { get; set; }
    }
}
