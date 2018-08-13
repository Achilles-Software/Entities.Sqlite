using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Linq;
using Achilles.Entities.Sqlite.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp
{
    public class TodoItemDatabase
    {
        readonly TodoDataContext _dataContext;

        public TodoItemDatabase( string dbPath )
        {
            var connectionString = "Data Source=" + dbPath;
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            _dataContext = new TodoDataContext( options );

            _dataContext.Database.Creator.CreateIfNotExists();
        }

        public Task<List<TodoItem>> GetItemsAsync()
        {
            return _dataContext.TodoItems.ToListAsync();
        }

        public Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            return _dataContext.TodoItems.Where( i => i.Done == false ).ToListAsync();
        }

        public Task<TodoItem> GetItemAsync( int id )
        {
            return _dataContext.TodoItems.FirstAsync( p => p.Id == 1 );
        }

        public Task<int> SaveItemAsync( TodoItem item )
        {
            if ( item.Id != 0 )
            {
                return _dataContext.TodoItems.UpdateAsync( item );
            }
            else
            {
                return _dataContext.TodoItems.AddAsync( item );
            }
        }

        public Task<int> DeleteItemAsync( TodoItem item )
        {
            return _dataContext.TodoItems.DeleteAsync( item );
        }
    }
}