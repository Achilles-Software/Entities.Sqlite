#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;

#endregion

namespace Achilles.Entities.Services
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
