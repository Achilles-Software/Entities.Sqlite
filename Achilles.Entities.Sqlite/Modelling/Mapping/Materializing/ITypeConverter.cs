using System;

namespace Achilles.Entities.Modelling.Mapping.Materializing
{
    public interface ITypeConverter
    {
        /// <summary>
        /// Converts the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value is to be converted to.</param>
        /// <returns>Converted value.</returns>
        object Convert( object value, Type type );

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value needs to be converted to.</param>
        /// <returns>Boolean response.</returns>
        bool CanConvert( object value, Type type );

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        int Order { get; }
    }
}
