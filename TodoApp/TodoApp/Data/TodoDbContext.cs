﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp
{
    public class TodoDbContext: DbContext
    {
        private static TestDbContext _dbContext;

        public DbTable<Product> Products { get; set; }

        public TestDbContext( DbContextOptions options ) : base( options )
        {
            Products = new DbTable<Product>( this );
        }

        public static TestDbContext Create( string connectionString )
        {
            if ( _dbContext == null )
            {
                var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

                _dbContext = new TestDbContext( options );
            }

            return _dbContext;
        }

        /// <summary>
        /// Overrided for context options configuration.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected internal override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );

            // Add context options configuration here...
        }

        /// <summary>
        /// Override for configuring entity mapping.
        /// </summary>
        /// <param name="config"></param>
        protected internal override void OnModelMapping( MappingConfiguration config )
        {
            config.Entity<Product>( builder =>
            {
                builder.ToTable( "Products" );

                builder.Property( p => p.Price )
                    .IsRequired();

                builder.Property( p => p.Salutation )
                    .Ignore();

                builder.Property( p => p.Id )
                    .IsKey();

                builder.Index( p => p.Name ).Name( "IX__Products_Name" ).IsUnique();

            } );

            base.OnModelMapping( config );
        }
    }

}