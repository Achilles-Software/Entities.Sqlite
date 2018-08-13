#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

namespace Achilles.Entities
{
    public class DataContextService : IDataContextService
    {
        public DataContext Instance { get; private set; }

        public IDataContextService Initialize( DataContext context )
        {
            Instance = context;
            return this;
        }
    }
}
