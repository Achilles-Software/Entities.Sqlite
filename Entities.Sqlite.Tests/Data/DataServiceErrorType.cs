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

using System.Collections.Generic;

#endregion

namespace Entities.Sqlite.Tests.Data
{
    /// <summary>
    /// Data service layer error types.
    /// </summary>
    public enum DataServiceErrorType : int
    {
        /// <summary>
        /// Indicates an unknown error type.
        /// </summary>
        Unknown = 0x01,

        /// <summary>
        /// Indicates entity validation error type.
        /// </summary>
        Validation = 0x02,

        /// <summary>
        /// Indicates a repository error type.
        /// </summary>
        Repository = 0x03,

        /// <summary>
        /// REVIEW: Should this be a sub of Repository error type?
        /// </summary>
        SqlReferentialConstraint = 547,
    }

    /// <summary>
    /// Data service errors as strings.
    /// </summary>
    public class DataServiceErrorString
    {
        /// <summary>
        /// Data service error type names.
        /// </summary>
        public static readonly IDictionary<DataServiceErrorType, string> Names = new Dictionary<DataServiceErrorType, string>
        {
            { DataServiceErrorType.Unknown, "Unknown access failure" },
            { DataServiceErrorType.Validation, "Validation failure" },
            { DataServiceErrorType.Repository, "Repository access failure" },
            { DataServiceErrorType.SqlReferentialConstraint, "SQL Referential Constraint violation" },
        };
    }
}
