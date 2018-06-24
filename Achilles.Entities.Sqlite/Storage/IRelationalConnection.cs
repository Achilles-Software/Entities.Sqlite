using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Achilles.Entities.Storage
{
    public interface IRelationalConnection : IDisposable
    {
        string ConnectionString { get; }

        DbConnection DbConnection { get; }

        Guid ConnectionId { get; }

        int? CommandTimeout { get; set; }

        //bool Open( bool errorsExpected = false );

        //Task<bool> OpenAsync( CancellationToken cancellationToken, bool errorsExpected = false );

        bool Close();
    }
}
