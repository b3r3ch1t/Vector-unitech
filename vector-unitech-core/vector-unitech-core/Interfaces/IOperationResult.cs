namespace vector_unitech_core.Interfaces
{
    public interface IOperationResult<TResult> : IOperationResult
    {
        TResult Result { get; set; }
    }

    public interface IOperationResult
    {
        bool Error { get; set; }
        string Message { get; set; }
    }
}