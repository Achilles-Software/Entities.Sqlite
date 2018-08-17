#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// Implements the <see cref="IIndexMapping"/> interface.
    /// </summary>
    public class IndexMapping : IIndexMapping
    {
        #region  Constructor(s)

        /// <summary>
        /// Constructs a IndexMapping instance from the provided propertyInfo parameter.
        /// </summary>
        /// <param name="indexInfo"></param>
        public IndexMapping( MemberInfo indexInfo )
        {
            IndexInfo = indexInfo;
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        internal MemberInfo IndexInfo { get; }

        /// <inheritdoc />
        public string PropertyName => IndexInfo.Name;

        /// <inheritdoc />
        //public string ColumnName
        //{
        //    get
        //    {
        //        // Since a property can be renamed through configuration, this property must be resolved after model building

        //        return IndexInfo.Name;
        //    }
        //}

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public bool IsUnique { get; set; }

        /// <inheritdoc />
        public int Order { get; set; }

        #endregion
    }
}
