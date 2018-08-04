#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Achilles.Entities.Configuration
{
    public abstract class DbContextOptions
    {
        public DbContextOptions()
        {
        }

        internal virtual void AddServices( IServiceCollection services )
        {
        }
    }
}
