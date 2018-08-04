using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApp
{
	public class TodoItemDatabase
	{
		readonly TodoDbContext _dbContext;

		public TodoItemDatabase(string dbPath)
		{
            var options = new DbContextOptionsBuilder().UseSqlite( dbPath ).Options;

            _dbContext = new TodoDbContext( options );
            _dbContext = new SQLiteAsyncConnection(dbPath);
			_dbContext.CreateTableAsync<TodoItem>().Wait();
		}

		public Task<List<TodoItem>> GetItemsAsync()
		{
			return _dbContext.Table<TodoItem>().ToListAsync();
		}

		public Task<List<TodoItem>> GetItemsNotDoneAsync()
		{
			return _dbContext.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

		public Task<TodoItem> GetItemAsync(int id)
		{
			return _dbContext.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(TodoItem item)
		{
			if (item.ID != 0)
			{
				return _dbContext.UpdateAsync(item);
			}
			else {
				return _dbContext.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(TodoItem item)
		{
			return _dbContext.DeleteAsync(item);
		}
	}
}

