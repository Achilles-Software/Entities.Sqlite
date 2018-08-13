#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Sqlite.Statements.Index;
using Achilles.Entities.Sqlite.Statements.Table;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Database
{
    internal class CreateDatabaseStatementBuilder : ISqlStatementBuilder<CreateDatabaseStatement>
    {
        #region Fields

        private readonly EntityMappingCollection _entityMappings;

        #endregion

        #region Constructor(s)

        public CreateDatabaseStatementBuilder( EntityMappingCollection mappingConfiguration )
        {
            _entityMappings = mappingConfiguration;
        }

        #endregion

        #region Public Methods

        public CreateDatabaseStatement BuildStatement()
        {
            var createTableStatements = GetCreateTableStatements();
            var createIndexStatements = GetCreateIndexStatements();
            var createStatements = createTableStatements.Concat<ISqlStatement>( createIndexStatements );
            var createDatabaseStatement = new CreateDatabaseStatement( createStatements );

            return createDatabaseStatement;
        }

        #endregion

        #region Private Methods

        private IEnumerable<CreateTableStatement> GetCreateTableStatements()
        {
            foreach ( var entityMapping in _entityMappings.Values )
            {
                var tableStatementBuilder = new CreateTableStatementBuilder( entityMapping );

                yield return tableStatementBuilder.BuildStatement();
            }
        }

        private IEnumerable<CreateIndexStatementCollection> GetCreateIndexStatements()
        {
            foreach ( var entityMapping in _entityMappings.Values )
            {
                var indexStatementBuilder = new CreateIndexStatementBuilder( entityMapping );

                yield return indexStatementBuilder.BuildStatement();
            }
        }

        #endregion
    }
}
