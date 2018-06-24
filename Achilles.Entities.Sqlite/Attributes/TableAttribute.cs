using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Attributes
{
    [AttributeUsage( AttributeTargets.Class )]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Flag whether to create the table without rowid (see https://sqlite.org/withoutrowid.html)
        ///
        /// The default is <c>false</c> so that sqlite adds an implicit <c>rowid</c> to every table created.
        /// </summary>
        public bool WithoutRowId { get; set; }

        public TableAttribute( string name )
        {
            Name = name;
        }
    }
}
