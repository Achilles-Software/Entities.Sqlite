using Achilles.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Achilles.Entities.Relational.Configuration
{
    public abstract class RelationalDbContextOptionsBuilder<TBuilder, TOption>  //IDbContextOptionsBuilder
        where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TOption>
        where TOption : RelationalOptions, new()
    {
        protected RelationalDbContextOptionsBuilder( DbContextOptionsBuilder optionsBuilder )
        {
            OptionsBuilder = optionsBuilder ?? throw new ArgumentNullException( nameof( optionsBuilder ) );
        }

        protected virtual DbContextOptionsBuilder OptionsBuilder { get; }

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
