#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling;
using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class ForeignKeyStatementBuilder : ISqlStatementBuilder<ColumnStatementCollection>
    {
        #region Private Fields

        private readonly IEntityModel _model;
        private readonly IEnumerable<IForeignKeyMapping> _foreignKeyMappings;

        #endregion

        #region Constructor(s)

        public ForeignKeyStatementBuilder( IEntityModel model, IEnumerable<IForeignKeyMapping> foreignKeyMappings )
        {
            _model = model;
            _foreignKeyMappings = foreignKeyMappings;
        }

        #endregion

        public ColumnStatementCollection BuildStatement()
        {
            var columnDefStatement = new ColumnStatementCollection( GetForeignKeyStatements().ToList() );

            return columnDefStatement;
        }

        private IEnumerable<ForeignKeyStatement> GetForeignKeyStatements()
        {
            foreach ( var foreignKeyMapping in _foreignKeyMappings )
            {
                var referenceInfo = foreignKeyMapping.ReferenceKeyProperty;
                var referenceEntityMapping = _model.GetEntityMapping( referenceInfo.DeclaringType );

                if ( referenceEntityMapping == null )
                {
                    // TODO: This should fail at validating foreign keys mappings after onModelBuilding!!!
                    throw new InvalidOperationException( "Foreign Key " );
                }

                yield return new ForeignKeyStatement
                {
                    ForeignKey = foreignKeyMapping.PropertyName,
                    ParentTable = referenceEntityMapping.TableName,
                    ParentKey = referenceInfo.Name,
                    CascadeDelete = foreignKeyMapping.CascadeDelete
                };
            }
        }
    }
}
