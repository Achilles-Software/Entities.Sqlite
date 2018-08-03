namespace Achilles.Entities.Relational
{
    /// <summary>
    /// Represents an SQL command parameter.
    /// </summary>
    public class SqlParameter
    {
        /// <summary>
        /// Constructs a new SQL parameter with a name, value pair.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public SqlParameter( string name, object value )
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the name of the parameter
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        public object Value { get; private set; }
    }
}
