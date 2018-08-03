#region Namespaces

using System.Data.Common;

#endregion

namespace Achilles.Entities.Relational.Configuration
{
    public interface IRelationalOptions
    {
        DbConnection Connection { get; }

        string ConnectionString { get; }

        int? CommandTimeout { get; }
    }
}
