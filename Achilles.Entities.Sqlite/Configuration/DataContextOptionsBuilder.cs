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
    public class DataContextOptionsBuilder
    {
        #region Constructor(s)

        public DataContextOptionsBuilder()
            : this( new DataContextOptions<DataContext>() )
        {
        }

        public DataContextOptionsBuilder( DataContextOptions options )
        {
            Options = options ?? throw new System.ArgumentNullException( nameof( options ) );
        }

        #endregion

        public DataContextOptions Options { get; internal set; }

        internal virtual void AddServices( IServiceCollection services )
        { }
    }
}
