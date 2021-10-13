using System;
using System.Collections.Generic;
using vector_unitech_core.Entities;
using vector_unitech_core.ValueObjects;

namespace vector_unitech_core.Data
{
    public interface IRepository : IDisposable
    {
        IEnumerable<Email> GetAllEmails();
        IEnumerable<TestEntity> GetNamesGroupedByHour();
    }
}