#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// Provides a fluent builder api to configure a <see cref="Mapping.ForeignKeyMapping"/>.
    /// </summary>
    public class ForeignKeyMappingBuilder : IForeignKeyMappingBuilder
    {
        #region Constructor(s)

        public ForeignKeyMappingBuilder( MemberInfo foreignKeyProperty )
        {
            ForeignKey = foreignKeyProperty ?? throw new ArgumentNullException( nameof( foreignKeyProperty ) );

            ForeignKeyMapping = CreateForeignKeyMapping( foreignKeyProperty );
        }

        #endregion

        /// <inheritdoc/>
        private MemberInfo ForeignKey { get; }

        /// <inheritdoc/>
        public IForeignKeyMapping ForeignKeyMapping { get; }

        /// <inheritdoc/>
        public IForeignKeyMappingBuilder Name( string constraintName )
        {
            ForeignKeyMapping.Name = constraintName;

            return this;
        }

        /// <inheritdoc/>
        public IForeignKeyMappingBuilder References<TEntity>( Expression<Func<TEntity, object>> referenceKeyLambda ) where TEntity : class
        {
            var referenceKey = ReflectionHelper.GetMemberInfo( referenceKeyLambda );

            ForeignKeyMapping.ReferenceKeyProperty = referenceKey;

            return this;
        }

        /// <inheritdoc/>
        public IForeignKeyMappingBuilder CascadeDelete()
        {
            ForeignKeyMapping.CascadeDelete = true;

            return this;
        }

        /// <inheritdoc/>
        internal IForeignKeyMapping Build() => ForeignKeyMapping;

        private IForeignKeyMapping CreateForeignKeyMapping( MemberInfo foreignKey ) => new ForeignKeyMapping( foreignKey );
    }
}
