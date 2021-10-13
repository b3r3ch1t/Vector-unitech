using System;
using vector_unitech_core.Interfaces;

namespace vector_unitech_core.Utils
{
    public class OperationResult<TResult> : OperationResult, IOperationResult<TResult>
    {

        public TResult Result { get; set; }

        public TResult ResultOrExceptionIfFailed()
        {
            if ( Error )
            {
                throw new Exception( Message );
            }

            return Result;
        }

        #region Ctors

        public OperationResult()
        {
        }

        public OperationResult( System.Net.HttpStatusCode statusCode, string message = null ) : base( statusCode, message )
        {
        }

        public OperationResult( string message ) : base( message )
        {
        }

        public OperationResult( Exception e ) : base( e )
        {
        }

        public OperationResult( Exception e, TResult result )
        {
            Result = result;
            Error = true;
            if ( e != null )
            {
                Message = e.Message;
            }
        }

        public OperationResult( OperationResult<TResult> operationResult )
        {
            Result = operationResult.Result;
            Error = operationResult.Error;
            Message = operationResult.Message;
        }


        public OperationResult( TResult result, bool error = false, string message = null )
        {
            Result = result;
            Error = error;
            if ( error )
            {
                Message = message;
            }
        }

        #endregion Ctors
    }
}
