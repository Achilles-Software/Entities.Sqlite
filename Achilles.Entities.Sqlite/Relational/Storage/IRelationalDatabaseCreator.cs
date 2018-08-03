#region Namespaces

using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Storage
{
    public interface IRelationalDatabaseCreator
    {
        bool CreateIfNotExists();

        Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken = default );

        string GenerateCreateScript();
    }
}
