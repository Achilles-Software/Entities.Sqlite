#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

#endregion

namespace Achilles.Entities.Relational.Configuration
{
    public abstract class RelationalDbContextOptionsBuilder<TBuilder, TOption>  //IDbContextOptionsBuilder
        where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TOption>
        where TOption : RelationalOptions, new()
    {
        protected RelationalDbContextOptionsBuilder( DataContextOptionsBuilder optionsBuilder )
        {
            OptionsBuilder = optionsBuilder ?? throw new ArgumentNullException( nameof( optionsBuilder ) );
        }

        protected virtual DataContextOptionsBuilder OptionsBuilder { get; }

        public virtual TBuilder CommandTimeout( int? commandTimeout )
            => WithOption( e => (TOption)e.WithCommandTimeout( commandTimeout ) );


        protected virtual TBuilder WithOption( Func<TOption, TOption> setAction )
        {
            var options = OptionsBuilder.Options;

            setAction( (TOption)OptionsBuilder.Options );

            return (TBuilder)this;
        }

        protected abstract void AddServices( IServiceCollection services );
    }
}
