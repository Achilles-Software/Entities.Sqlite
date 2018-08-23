#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Linq
{
    public sealed class EntityReference<TEntity> : IEntityReference<TEntity>, IEntityReference
        where TEntity : class
    {
        #region Private Fields

        private IEnumerable<TEntity> _source;
        private TEntity _entity;

        private bool _isLoaded = false;

        #endregion

        #region Constructor(s)

        public EntityReference()
        {
            var d = 6;
        }

        #endregion

        #region Public API

        public TEntity Entity
        {
            get
            {
                if ( _isLoaded )
                    return _entity;
                else
                {
                    Load();
                    return _entity;
                }
            }
        }

        // TJT: Make a direct internal property (Source) reference through a compiled lambda expression
        public void AttachSource( IEnumerable<TEntity> source )
        {
            _source = source;
        }

        #endregion

        #region Private Methods

        private void Load()
        {
            if ( !_isLoaded && _source != null )
            {
                _entity = Enumerable.SingleOrDefault( _source );
                _isLoaded = true;
            }
        }
        
        #endregion
    }
}
