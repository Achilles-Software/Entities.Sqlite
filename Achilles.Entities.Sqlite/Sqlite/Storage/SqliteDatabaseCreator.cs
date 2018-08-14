#region Namespaces

using Achilles.Entities.Sqlite.SqlStatements.Database;
using Achilles.Entities.Storage;
using System.Transactions;

#endregion

namespace Achilles.Entities.Sqlite.Storage
{
    /// <summary>
    /// Sqlite specific implementation of the abstract <see cref="RelationalDatabaseCreator"/> base class.
    /// </summary>
    public class SqliteDatabaseCreator : RelationalDatabaseCreator
    {
        #region Fields

       private readonly SqliteRelationalConnection _connection;

        #endregion

        #region Constructor(s)

        public SqliteDatabaseCreator( IDataContextService dbContext, IRelationalConnection connection )
            : base( dbContext )
        {
            _connection = connection as SqliteRelationalConnection;
        }

        #endregion

        #region Public API Methods

        public override bool CreateIfNotExists()
        {
            using ( new TransactionScope( TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled ) )
            {
                if ( !_dbContext.Instance.Database.Exists() )
                {
                    _dbContext.Instance.Database.Create();

                    CreateDatabase();

                    return true;
                }

                if ( !_dbContext.Instance.Database.HasTables() )
                {
                    CreateDatabase();

                    return true;
                }
            }

            return false;
        }

        public override string GenerateCreateScript()
        {
            return GetCreateDatabaseCommands();
        }

        #endregion

        #region Private Methods

        private void CreateDatabase()
        {
            var createDatabaseCommand = GetCreateDatabaseCommands();

            using ( var command = _connection.DbConnection.CreateCommand() )
            {
                command.CommandText = createDatabaseCommand;

                command.ExecuteNonQuery();
            }
        }

        private string GetCreateDatabaseCommands()
        {
            var databaseBuilder = new CreateDatabaseStatementBuilder( _dbContext.Instance.Model.EntityMappings );

            return databaseBuilder.BuildStatement().GetText();
        }

        #endregion
    }
}
