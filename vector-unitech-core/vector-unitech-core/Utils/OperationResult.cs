using System;
using System.Net;
using vector_unitech_core.Interfaces;

namespace vector_unitech_core.Utils
{
    public class OperationResult : IOperationResult
    {
        public bool Error { get; set; }


        public string Message { get; set; }


        public HttpStatusCode HttpStatusCode { get; set; }

        public void ExceptionIfFailed()
        {
            if ( Error )
            {
                throw new Exception( Message );
            }
        }

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref = "OperationResult{TResult}" /> class.
        /// </summary>
        public OperationResult() { }

        public OperationResult( HttpStatusCode statusCode, string error = null ) : this()
        {
            Error = true;
            HttpStatusCode = statusCode;
            Message = error;
        }

        public OperationResult( string error )
            : this()
        {
            Error = true;
            Message = error;
        }

        public OperationResult( Exception e )
            : this()
        {
            Error = true;
            if ( e != null )
            {
                Message = e.Message;
            }
        }

        #endregion Ctors
    }
}
