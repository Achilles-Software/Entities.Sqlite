#region Namespaces

using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Storage
{
    public interface IRelationalDatabase
    {
        IRelationalConnection Connection { get; }

        IRelationalDatabaseCreator Creator { get; }

        bool Exists();

        Task<bool> ExistsAsync( CancellationToken cancellationToken );

        void Create();

        Task CreateAsync( CancellationToken cancellationToken );

        bool Delete();

        Task<bool> DeleteAsync( CancellationToken cancellationToken );

        bool HasTables();

        Task<bool> HasTablesAsync( CancellationToken cancellationToken );
    }
}
