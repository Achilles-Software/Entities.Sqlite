#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Configuration
{
    public class DbContextOptions<TContext> : DbContextOptions where TContext : DbContext
    {
        public DbContextOptions()
        {
        }

        #region Properties

        public DbContextOptions Options { get; internal set; }

        #endregion
    }
}
