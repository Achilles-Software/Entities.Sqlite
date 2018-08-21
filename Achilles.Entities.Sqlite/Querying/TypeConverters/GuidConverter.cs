#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;

#endregion

namespace Achilles.Entities.Querying.TypeConverters
{
    /// <summary>
    /// Converts values to Guids.
    /// </summary>
    public class GuidConverter : ITypeConverter
    {
        #region Implementation of ITypeConverter

        /// <summary>
        /// Converts the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value is to be converted to.</param>
        /// <returns>Converted value.</returns>
        public object Convert( object value, Type type )
        {
            object convertedValue = null;

            if ( value is string )
            {
                convertedValue = new Guid( value as string );
            }
            if ( value is byte[] )
            {
                convertedValue = new Guid( value as byte[] );
            }

            return convertedValue;
        }

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value needs to be converted to.</param>
        /// <returns>Boolean response.</returns>
        public bool CanConvert( object value, Type type )
        {
            var conversionType = Nullable.GetUnderlyingType( type ) ?? type;
            return conversionType == typeof( Guid );
        }

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        public int Order { get { return 100; } }

        #endregion
    }
}
