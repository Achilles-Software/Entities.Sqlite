#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
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
