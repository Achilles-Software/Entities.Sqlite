using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Relational.Query
{
    public interface IAsyncEnumerableAccessor<out T>
    {
        IAsyncEnumerable<T> AsyncEnumerable { get; }
    }
}
