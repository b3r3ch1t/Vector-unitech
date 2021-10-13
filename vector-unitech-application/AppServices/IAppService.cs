using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vector_unitech_application.Models;
using vector_unitech_core.Utils;

namespace vector_unitech_application.AppServices
{
    public interface IAppService : IDisposable
    {
        Task<OperationResult<IEnumerable<string>>> GetAllEmailsAsync();
        Task<OperationResult<IEnumerable<GroupedByHourModel>>> GetNamesGroupedByHourAsync();
    }
}