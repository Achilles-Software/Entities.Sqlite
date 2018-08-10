using Achilles.Entities.Relational.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Sqlite.Tests.Data
{
    public class TestDbRepository
    {
        #region Fields, Constants

        private static TestDbContext _context;

        #endregion

        #region Constructor(s)

        public TestDbRepository()
        {
            // TJT: Review this. 
            if ( _context == null )
            {
                throw new System.Exception( "The database context is null." );
            }
        }

        public TestDbRepository( string databasePath )
        {
            // Creates or gets the database context singleton
            _context = TestDbContext.Create( databasePath );
        }

        #endregion

        #region Public Query Methods

        public async Task<Product> GetAsync( int id )
        {
            return await _context.Products.FirstAsync( item => item.Id == id );
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        #endregion

        #region Public CRUD Methods

        public async Task<DataServiceResult> AddAsync( Product item )
        {
            try
            {
                if ( item.Id == 0 )
                {
                    await _context.Products.AddAsync( item );
                }
            }
            catch ( Exception e )
            {
                return DataServiceResult.Failed( DataServiceErrorType.Repository, e );
            }

            return DataServiceResult.Success;
        }

        public async Task<DataServiceResult> DeleteAsync( Product item )
        {
            try
            {
                await _context.Products.DeleteAsync( item );
            }
            catch ( Exception e )
            {
                return DataServiceResult.Failed( DataServiceErrorType.Repository, e );
            }

            return DataServiceResult.Success;
        }

        #endregion
    }
}
