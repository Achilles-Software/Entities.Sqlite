using Achilles.Entities.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Relational.Modelling
{
    public interface IRelationalModel
    {
        EntityMappingCollection EntityMappings { get; }
    }
}
