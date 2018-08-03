using System.Collections.Generic;

namespace Achilles.Entities.Relational.Statements
{
    public interface ISqlStatementCollection : ISqlStatement, ICollection<ISqlStatement>
    {
    }
}
