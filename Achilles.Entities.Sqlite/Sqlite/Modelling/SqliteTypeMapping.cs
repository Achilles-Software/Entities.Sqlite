#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Mapping
{
    public class SqliteTypeMapping
    {
        // Available Sqlite data types...
        private const string IntegerTypeName = "INTEGER";
        private const string RealTypeName = "REAL";
        private const string BlobTypeName = "BLOB";
        private const string TextTypeName = "TEXT";

        private static readonly TypeMapping _integer = new TypeMapping( typeof( long ), IntegerTypeName );
        private static readonly TypeMapping _real = new TypeMapping( typeof( double ), RealTypeName );
        private static readonly TypeMapping _blob = new TypeMapping( typeof( byte[] ), BlobTypeName );
        private static readonly TypeMapping _text = new TypeMapping( typeof( string ), TextTypeName );

        private static readonly Dictionary<Type, TypeMapping> _TypeMappings = new Dictionary<Type, TypeMapping>
            {
                { typeof(string), _text },
                { typeof(byte[]), _blob },
                { typeof(bool), _integer  },
                { typeof(byte), _integer },
                { typeof(char), _integer },
                { typeof(int), _integer },
                { typeof(long), _integer },
                { typeof(sbyte), _integer },
                { typeof(short), _integer },
                { typeof(uint), _integer },
                { typeof(ulong), _integer },
                { typeof(ushort), _integer },
                { typeof(DateTime), _text },
                { typeof(DateTimeOffset), _text },
                { typeof(TimeSpan), _text },
                { typeof(decimal), _text }, // TJT: Huh???
                { typeof(double), _real },
                { typeof(float),_real },
                { typeof(Guid), _blob }
            };

        public static TypeMapping FindMapping( Type type )
        {
            if ( _TypeMappings.TryGetValue( type, out var mapping ) )
            {
                return mapping;
            }

            return null;
        }
    }
}
