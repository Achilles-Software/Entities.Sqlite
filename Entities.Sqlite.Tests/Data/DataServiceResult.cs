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
    /// Data service layer API call result.
    /// </summary>
    public class DataServiceResult
    {
        #region Fields

        private static readonly DataServiceResult _success = new DataServiceResult( true );

        #endregion

        #region Constructors

        private DataServiceResult( bool success )
        {
            Succeeded = success;
        }

        /// <summary>
        /// Creates a new <see cref="DataServiceResult"/> with the provided <see cref="DataServiceError"/> error.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="error"></param>
        public DataServiceResult( DataServiceErrorType errorType, DataServiceError error )
        {
            Succeeded = false;
            ErrorType = errorType;
            Error = error;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data service layer error cause.
        /// </summary>
        public DataServiceError Error { get; private set; }

        /// <summary>
        /// Gets the service layer error type.
        /// </summary>
        public DataServiceErrorType ErrorType { get; private set; }

        /// <summary>
        /// Indicates that the service layer call was successful.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Gets the data service layer success result.
        /// </summary>
        public static DataServiceResult Success
        {
            get
            {
                return _success;
            }
        }

        /// <summary>
        /// Creates a failed <see cref="DataServiceResult"/> with the <see cref="Exception"/> cause.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static DataServiceResult Failed( DataServiceErrorType errorType, Exception e = null )
        {
            return new DataServiceResult( errorType, new DataServiceError( string.Empty, e ) );
        }

        /// <summary>
        /// Creates a failed <see cref="DataServiceResult"/> with the <see cref="DataServiceError"/> cause.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static DataServiceResult Failed( DataServiceErrorType errorType, DataServiceError error = null )
        {
            return new DataServiceResult( errorType, error );
        }

        #endregion
    }
}
