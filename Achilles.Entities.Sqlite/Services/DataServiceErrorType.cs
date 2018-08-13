#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Services
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
