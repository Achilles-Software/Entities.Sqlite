#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Relational.SqlStatements;
using Achilles.Entities.Sqlite.SqlStatements;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Sqlite.SqlStatements.Table
{
    internal class ColumnStatementCollectionBuilder : ISqlStatementBuilder<ColumnStatementCollection>
    {
        private readonly IEnumerable<IColumnMapping> properties;
        private readonly IEnumerable<IColumnMapping> keyMembers;

        public ColumnStatementCollectionBuilder( IEnumerable<IColumnMapping> properties, IEnumerable<IColumnMapping> keyMembers)
        {
            this.properties = properties;
            this.keyMembers = keyMembers;
        }

        public ColumnStatementCollection BuildStatement()
        {
            var columnDefStatement = new ColumnStatementCollection(CreateColumnStatements().ToList());
            return columnDefStatement;
        }

        private IEnumerable<ColumnStatement> CreateColumnStatements()
        {
            foreach (var property in properties)
            {
                var columnStatement = new ColumnStatement
                {
                    ColumnName = property.ColumnName,
                    TypeName = property.ColumnDataType,

                    ColumnConstraints = new ColumnConstraintCollection()
                };

                AddMaxLengthConstraintIfNecessary(property, columnStatement);
                AdjustDatatypeForAutogenerationIfNecessary(property, columnStatement);
                AddNullConstraintIfNecessary(property, columnStatement);
                AddUniqueConstraintIfNecessary(property, columnStatement);
                AddCollationConstraintIfNecessary(property, columnStatement);
                AddPrimaryKeyConstraintAndAdjustTypeIfNecessary(property, columnStatement);
                AddDefaultValueConstraintIfNecessary(property, columnStatement);

                yield return columnStatement;
            }
        }

        private static void AddMaxLengthConstraintIfNecessary( IColumnMapping property, ColumnStatement columnStatement )
        {
            if ( property.MaxLength.HasValue )
            {
                columnStatement.ColumnConstraints.Add( new MaxLengthConstraint( property.MaxLength.Value ) );
            }
        }

        private static void AdjustDatatypeForAutogenerationIfNecessary(IColumnMapping property, ColumnStatement columnStatement)
        {
            // FIXME:

            //if (property.StoreGeneratedPattern == StoreGeneratedPattern.Identity)
            //{
            //    // Must be INTEGER else SQLite will not generate the Ids
            //    ConvertIntegerType(columnStatement);
            //}
        }

        private static void AddNullConstraintIfNecessary( IColumnMapping property, ColumnStatement columnStatement)
        {
            // TJT: Review this
            if ( property.IsRequired ) 
            {
                // Only mark it as NotNull if it should not be generated.
                columnStatement.ColumnConstraints.Add( new NotNullConstraint() );
            }
        }

        private static void AddCollationConstraintIfNecessary(IColumnMapping property, ColumnStatement columnStatement)
        {
            // FIXME:

            //var value = property.GetCustomAnnotation<CollateAttribute>();
            //if (value != null)
            //{
            //    columnStatement.ColumnConstraints.Add(new CollateConstraint { CollationFunction = value.Collation, CustomCollationFunction = value.Function });
            //}
        }

        private static void AddUniqueConstraintIfNecessary( IColumnMapping property, ColumnStatement columnStatement )
        {
            var value = property.IsUnique;

            if ( property.IsUnique )
            {
                var conflictAction = ConflictAction.None;
                columnStatement.ColumnConstraints.Add( new UniqueConstraint { ConflictAction = conflictAction } );
            }
        }

        private static void AddDefaultValueConstraintIfNecessary( IColumnMapping property, ColumnStatement columnStatement)
        {
            if ( !string.IsNullOrEmpty( property.DefaultValue ) )
            {
                columnStatement.ColumnConstraints.Add( new DefaultValueConstraint { DefaultValue = property.DefaultValue } );
            }
        }

        private void AddPrimaryKeyConstraintAndAdjustTypeIfNecessary(IColumnMapping property, ColumnStatement columnStatement)
        {
            // Only handle a single primary key this way.

            if ( keyMembers.Count() != 1 || !property.Equals( keyMembers.Single() ) )
            {
                return;
            }

            ConvertIntegerType( columnStatement );
            var primaryKeyConstraint = new PrimaryKeyConstraint();

            // FIXME: primaryKeyConstraint.Autoincrement = property.Autoincrement() != null;

            columnStatement.ColumnConstraints.Add( primaryKeyConstraint );
        }

        private static void ConvertIntegerType(ColumnStatement columnStatement)
        {
            const string integerType = "INTEGER";
            columnStatement.TypeName = columnStatement.TypeName.ToUpperInvariant() == "INT" ? integerType : columnStatement.TypeName;
        }
    }
}
