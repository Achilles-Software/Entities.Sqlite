using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Linq
{
    internal interface IEntityReferenceSource
    {
        bool HasSource { get; }

        void SetSource<TSource>( IEnumerable<TSource> source ) where TSource : class;
    }
}
