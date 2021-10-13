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
            try
            {
                var valueCached = await _cacheRepository.GetValueFromKeyAsync( _key );


                IEnumerable<Email> result;
                if ( valueCached == null || valueCached.Error || !valueCached.Result.Any() || valueCached.Result == null )
                {
                    var responseMockApi = new List<TestEntity>();

                    try
                    {
                        responseMockApi = ( await _mockApi.GetAllAsync() ).Result.ToList();

                        var expiryDate = DateTime.Now.AddDays( 1 );

                        var response = await _cacheRepository.SetValueToKey( _key, responseMockApi, expiryDate );

                        if ( response.Error )
                        {
                            _error.Error( response.Message );
                        }
                    }
                    catch ( Exception e )
                    {
                        _error.Error( e );
                    }

                    result = responseMockApi
                        .Select( x => new Email( endereco: x.Mail, nome: x.Name ) );

                    return result;
                }

                result = JsonSerializer.Deserialize<IEnumerable<TestEntity>>( valueCached.Result )
                    .Select( x => new Email( endereco: x.Mail, nome: x.Name ) );

                return result;
            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new List<Email>();
        }

        public Task<IEnumerable<TestEntity>> GetNamesGroupedByHourAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

            _error?.Dispose();
            _mockApi?.Dispose();
            GC.SuppressFinalize( this );
        }
    }
}
