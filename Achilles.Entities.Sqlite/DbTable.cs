#region Copyright Notice

// Copyright (c) by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

#endregion

#region Namespaces

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities
{
    public class DbTable<TEntity> : IDbTable<TEntity>, IQueryable<TEntity>, IListSource
        where TEntity : class
    {
        #region Fields

        private DbContext _context;

        #endregion

        #region Constructor(s)

        public DbTable( DbContext context )
        {
            _context = context;
        }

        #endregion

        public bool ContainsListCollection => throw new NotImplementedException();

        public Type ElementType => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IList GetList()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
