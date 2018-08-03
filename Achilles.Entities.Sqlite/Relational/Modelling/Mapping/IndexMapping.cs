using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Achilles.Entities.Mapping
{
    public class IndexMapping : IIndexMapping
    {
        #region  Constructor(s)

        public IndexMapping( PropertyInfo propertyInfo )
        {
            PropertyInfo = propertyInfo;
        }

        #endregion

        #region Public Properties

        public PropertyInfo PropertyInfo { get; }

        public string PropertyName => PropertyInfo.Name;

        public string Name { get; set; }

        public bool IsUnique { get; set; }

        public int Order { get; set; }

        #endregion
    }
}
