#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.Statements.Insert
{
    internal class InsertStatementBuilder : ISqlStatementBuilder<InsertStatement>
    {
        private object _entity;
        private readonly IEntityMapping _entityMapping;

        public InsertStatementBuilder( object entity, IEntityMapping entityMapping )
        {
            _entity = entity;
            _entityMapping = entityMapping;
        }

        public InsertStatement BuildStatement()
        {
            var columnNameCollection = new ColumnNameStatementCollectionBuilder( _entityMapping ).BuildStatement();
            
            var columnNameStatements = new List<ISqlStatement>();
            columnNameStatements.AddRange( columnNameCollection );

            var columnParametersBuilder = new ColumnParameterStatementCollectionBuilder( _entity, _entityMapping );;

            var columnParameterStatements = new List<ISqlStatement>();
            columnParameterStatements.AddRange( columnParametersBuilder.BuildStatement() );

            return new InsertStatement
            {
                TableName = NameCreator.EscapeName( _entityMapping.TableName ),

                ColumnNameStatementCollection = new ColumnNameStatementCollection( columnNameStatements ),

                ColumnParameterStatementCollection = new ColumnParameterStatementCollection( columnParameterStatements ),

                Parameters = columnParametersBuilder.Parameters
            };
        }
    }
}
