#region Namespaces

using System;

#endregion

namespace Achilles.Entities.Mapping
{
    public class TypeMapping
    {
        #region Constructor(s)

        public TypeMapping( Type type, string mappingTypeName )
        {
            Type = type;
            MappingTypeName = mappingTypeName; 
        }

        #endregion

        #region Public Properties

        public Type Type { get; private set; }

        public string MappingTypeName { get; private set; }

        #endregion
    }
}
