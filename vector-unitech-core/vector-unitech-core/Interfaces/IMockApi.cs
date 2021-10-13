using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.Utils;

namespace vector_unitech_core.Interfaces
{
    public interface IMockApi : IDisposable
    {
        Task<OperationResult<IEnumerable<TestEntity>>> GetAllAsync();
    }
}