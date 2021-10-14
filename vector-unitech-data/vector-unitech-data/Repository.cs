using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.Interfaces;
using vector_unitech_core.ValueObjects;

namespace vector_unitech_data
{
    public class Repository : IRepository
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IError _error;
        private readonly IMockApi _mockApi;
        private const string _key = "GetMockApi_";

        public Repository( ICacheRepository cacheRepository, IError error, IMockApi mockApi )
        {
            _cacheRepository = cacheRepository;
            _error = error;
            _mockApi = mockApi;
        }

        public async Task<IEnumerable<Email>> GetAllEmailsAsync()
        {
            var result = await GetAllAsync();

            return result.Select( x => new Email( endereco: x.Mail, nome: x.Name ) );

        }

        public async Task<IEnumerable<GroupedByHour>> GetNamesGroupedByHourAsync()
        {
            var response = await GetAllAsync();

            var result = response
                .GroupBy( row => new
                {
                    Date = row.CreatedAt.Date,
                    Hour = row.CreatedAt.Hour
                } )
                .Select( grp => new
                    GroupedByHour
                {
                    CreatedAt = grp.Key.Date,
                    ListEntity = grp.ToList()
                } );

            return result;
        }

        public async Task<IEnumerable<TestEntity>> GetAllAsync()
        {
            try
            {
                var valueCached = await _cacheRepository.GetValueFromKeyAsync( _key );

                if ( valueCached != null && !valueCached.Error && valueCached.Result.Any() && valueCached.Result != null )

                    return JsonSerializer.Deserialize<IEnumerable<TestEntity>>( valueCached.Result );


                try
                {
                    var responseMockApi = ( await _mockApi.GetAllAsync() ).Result.ToList();
                    await _cacheRepository.SetValueToKey( _key, responseMockApi );

                    return responseMockApi;
                }
                catch ( Exception e )
                {
                    _error.Error( e );
                    return new List<TestEntity>();
                }


            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new List<TestEntity>();
        }

        public void Dispose()
        {
            _cacheRepository?.Dispose();
            _error?.Dispose();
            _mockApi?.Dispose();
            GC.SuppressFinalize( this );
        }
    }
}
