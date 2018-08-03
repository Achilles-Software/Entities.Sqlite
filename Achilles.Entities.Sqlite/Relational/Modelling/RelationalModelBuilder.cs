using Achilles.Entities.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Relational.Modelling
{
    public abstract class RelationalModelBuilder : IRelationalModelBuilder
    {
        public abstract IRelationalModel Build( DbContext context );
    }
}
