using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Achilles.Entities;

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
            try
            {
                //var query = from p in connection.Queryable<Session>() select p;
                //var list = query.ToList();

                var query = from p in _context.Products select p;

                var result = query.ToList();

                return await _context.Products.ToListAsync();
            }
            catch ( Exception e )
            {
                int step = 1;
            }

            return null;
        }

        #endregion

        #region Public CRUD Methods

        public async Task<DataServiceResult> AddAsync( Product item )
        {
            try
            {
                if ( item.Id == 0 )
                {
                    //await _context.Products.AddAsync( item );
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
            //try
            //{
            //    _context.Sessions.Remove( item );

            //    await _context.SaveChangesAsync();
            //}
            //catch ( DbUpdateException e )
            //{
            //    return DataServiceResult.Failed( DataServiceErrorType.Repository, e );
            //}

            return DataServiceResult.Success;
        }

        #endregion
    }

}
