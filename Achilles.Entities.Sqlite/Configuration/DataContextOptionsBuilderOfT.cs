#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

namespace Achilles.Entities.Configuration
{
    public class DataContextOptionsBuilder<TContext> : DataContextOptionsBuilder where TContext : DataContext
    {
        #region Constructor(s)

        public DataContextOptionsBuilder()
            : this( new DataContextOptions<TContext>() )
        {
        }

        public DataContextOptionsBuilder( DataContextOptions<TContext> options )
            : base( options )
        {
        }

        #endregion

        public new virtual DataContextOptions<TContext> Options => (DataContextOptions<TContext>)base.Options;
    }
}
