using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;

namespace vector_unitech_service
{
    public class MockApi : IMockApi
    {
        public void Dispose()
        {
            GC.SuppressFinalize( this );
        }

        public async Task<OperationResult<IEnumerable<TestEntity>>> GetAllAsync()
        {
            var client = new RestClient( "https://6064ac2bf09197001778660d.mockapi.io/" );
            var request = new RestRequest( "api/test-api", Method.GET );
            var queryResult = await client.ExecuteAsync<IEnumerable<TestEntity>>( request );

            return new OperationResult<IEnumerable<TestEntity>>( queryResult.Data );

        }
    }
}
