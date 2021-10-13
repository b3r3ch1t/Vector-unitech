using System;
using System.Collections.Generic;
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


        public Repository( ICacheRepository cacheRepository, IError error, IMockApi mockApi )
        {
            _cacheRepository = cacheRepository;
            _error = error;
            _mockApi = mockApi;
        }

        public Task<IEnumerable<Email>> GetAllEmailsAsync( string key )
        {
            try
            {
                var value = _cacheRepository.GetValueFromKeyAsync( key );
                if ( value == null )
                {
                    value = ( IEnumerable<Email> ) _mockApi.GetAllAsync();

                    var expiryDate = DateTime.Now.AddDays( 1 );

                    var exiryTimeSpan = expiryDate.Subtract( new DateTime( 1970, 01, 01 ) ).TotalSeconds;

                    _cacheRepository.SetValueToKey( key, value, exiryTimeSpan );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                throw;
            }
        }

        public Task<IEnumerable<TestEntity>> GetNamesGroupedByHourAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
