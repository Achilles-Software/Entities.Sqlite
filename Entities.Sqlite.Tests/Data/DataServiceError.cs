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

#endregion

namespace Entities.Sqlite.Tests.Data
{
    /// <summary>
    /// A <see cref="DataServiceResult"/> error.
    /// </summary>
    public class DataServiceError
    {
        #region Constructor(s)

        /// <summary>
        /// Creates a new ServiceError with the specific <see cref="Exception"/> cause.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exception"></param>
        public DataServiceError( string key, Exception exception )
            : this( key, exception, errorMessage: null )
        {
            if ( key == null )
            {
                throw new ArgumentNullException( nameof( key ) );
            }

            if ( exception == null )
            {
                throw new ArgumentNullException( nameof( exception ) );
            }
        }

        /// <summary>
        /// Creates a new <see cref="DataServiceError"/> with the specific <see cref="Exception"/> and error message.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exception"></param>
        /// <param name="errorMessage"></param>
        public DataServiceError( string key, Exception exception, string errorMessage )
            : this( key, errorMessage )
        {
            Exception = exception ?? throw new ArgumentNullException( nameof( exception ) );
        }

        /// <summary>
        /// Creates a new <see cref="DataServiceError"/> with the specific error message.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorMessage"></param>
        public DataServiceError( string key, string errorMessage )
        {
            Key = key ?? throw new ArgumentNullException( nameof( key ) );

            ErrorMessage = errorMessage ?? string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the service error exception cause.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the service error message.
        /// </summary>
        public string ErrorMessage { get; }

        #endregion
    }
}
