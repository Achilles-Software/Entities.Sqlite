#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Data.Common;

#endregion

namespace Achilles.Entities.Relational.Configuration
{
    public interface IRelationalOptions
    {
        DbConnection Connection { get; }

        string ConnectionString { get; }

        int? CommandTimeout { get; }
    }
}
