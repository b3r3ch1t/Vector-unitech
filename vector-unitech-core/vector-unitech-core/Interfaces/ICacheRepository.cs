using System;
using System.Threading.Tasks;
using vector_unitech_core.Utils;

namespace vector_unitech_core.Interfaces
{
    public interface ICacheRepository : IDisposable
    {
        Task<OperationResult<string>> GetValueFromKeyAsync( string key );
        Task<OperationResult<bool>> SetValueToKey<T>( string key, T value, DateTime expires );
    }
}
