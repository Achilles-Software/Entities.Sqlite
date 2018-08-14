#region Namespaces

using Achilles.Entities.Linq;
using Achilles.Entities.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    public class TestRepository
    {
        #region Fields, Constants

        private static TestDataContext _context;

        #endregion

        #region Constructor(s)

        public TestRepository()
        {
            // TJT: Review this. 
            if ( _context == null )
            {
                throw new System.Exception( "The database context is null." );
            }
        }

        public TestRepository( string databasePath )
        {
            // Creates or gets the database context singleton
            _context = TestDataContext.Create( databasePath );
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
