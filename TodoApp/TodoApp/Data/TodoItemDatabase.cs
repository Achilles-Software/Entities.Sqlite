using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Query;
using Achilles.Entities.Sqlite.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp
{
    public class TodoItemDatabase
	{
		readonly TodoDbContext _dbContext;

		public TodoItemDatabase(string dbPath)
		{
            var connectionString = "Data Source=" + dbPath;
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            _dbContext = new TodoDbContext( options );

            _dbContext.Database.Creator.CreateIfNotExists();
		}

		public Task<List<TodoItem>> GetItemsAsync()
		{
			return _dbContext.TodoItems.ToListAsync();
		}

		public Task<List<TodoItem>> GetItemsNotDoneAsync()
		{
            return _dbContext.TodoItems.Select( i => i ).Where( i => i.Done == false ).ToListAsync();
		}

		public Task<TodoItem> GetItemAsync(int id)
		{
            return _dbContext.TodoItems.FirstAsync( p => p.Id == 1 );
        }

		public Task<int> SaveItemAsync(TodoItem item)
		{
			if (item.Id != 0)
			{
				return _dbContext.TodoItems.UpdateAsync(item);
			}
			else {
				return _dbContext.TodoItems.AddAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(TodoItem item)
		{
			return _dbContext.TodoItems.DeleteAsync(item);
		}
	}
}

