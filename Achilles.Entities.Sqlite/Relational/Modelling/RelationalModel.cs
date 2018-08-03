#region Namespaces

using Achilles.Entities.Mapping;
using System;

#endregion

namespace Achilles.Entities.Relational.Modelling
{
    public class RelationalModel : IRelationalModel
    {
        public RelationalModel( EntityMappingCollection entityMappings ) 
            => EntityMappings = entityMappings ?? throw new ArgumentNullException( nameof( entityMappings ) );

        public EntityMappingCollection EntityMappings { get; }

    }
}
