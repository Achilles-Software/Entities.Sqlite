#region Namespaces

using Achilles.Entities.Relational.SqlStatements;
using System;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class MaxLengthConstraint : ISqlStatement
    {
        private const string Template = "({max-length})";

        public int? MaxLength { get; set; }

        public MaxLengthConstraint() { }

        public MaxLengthConstraint( int maxLength )
        {
            MaxLength = maxLength;
        }

        public string GetText()
        {
            if ( MaxLength == null )
            {
                throw new InvalidOperationException( "MaxLength must not be null!" );
            }

            return Template.Replace( "{max-length}", MaxLength.ToString() );
        }
    }
}
