using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.ValueObjects;

namespace vector_unitech_core.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task<IEnumerable<Email>> GetAllEmailsAsync();
        Task<IEnumerable<TestEntity>> GetAllAsync();
        Task<IEnumerable<TestEntity>> GetNamesGroupedByHourAsync();
    }
}