#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Modelling;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class CreateTableStatementBuilder : ISqlStatementBuilder<CreateTableStatement>
    {
        #region Private Fields

        private readonly IEntityModel _model;
        private readonly IEntityMapping _entityMapping;

        #endregion

        public CreateTableStatementBuilder( IEntityModel model, IEntityMapping entityMapping )
        {
            _model = model ?? throw new System.ArgumentNullException( nameof( model ) );
            _entityMapping = entityMapping ?? throw new System.ArgumentNullException( nameof( entityMapping ) );
        }

        public CreateTableStatement BuildStatement()
        {
            var keyMembers = _entityMapping.ColumnMappings.Where( p => p.IsKey ).ToList();

            // Only create a CompositePrimaryKeyStatement if there is a composite primary key.
            // If there is just one key member this is handled using a constraint.
            CompositePrimaryKeyStatement compositePrimaryKeyStatement = null;

            if ( keyMembers.Count > 1 )
            {
                compositePrimaryKeyStatement = new CompositePrimaryKeyStatementBuilder( keyMembers ).BuildStatement();
            }

            var simpleColumnCollection = new ColumnStatementCollectionBuilder( _entityMapping.ColumnMappings, keyMembers ).BuildStatement();
            
            var foreignKeyCollection = new ForeignKeyStatementBuilder( _model, _entityMapping.ForeignKeyMappings ).BuildStatement();

            var columnStatements = new List<ISqlStatement>();
            columnStatements.AddRange( simpleColumnCollection );
            columnStatements.AddIfNotNull( compositePrimaryKeyStatement );
            columnStatements.AddRange( foreignKeyCollection );

            return new CreateTableStatement
            {
                TableName = NameCreator.EscapeName( _entityMapping.TableName ),
                ColumnStatementCollection = new ColumnStatementCollection( columnStatements )
            };
        }
    }
}
