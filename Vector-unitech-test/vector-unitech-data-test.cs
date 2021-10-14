using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;
using vector_unitech_data;
using Xunit;

namespace Vector_unitech_test
{
    public class VectorUnitechDataTest
    {
        private readonly List<TestEntity> _listTestEntities;
        private readonly Mock<ICacheRepository> _cacheRepositoryMock;
        private readonly Mock<IError> _error;
        private readonly Mock<IMockApi> _mockApi;
        private const string _key = "GetMockApi_";
        private readonly IRepository _repository;

        public VectorUnitechDataTest()
        {
            _listTestEntities = MockApiFaker.FakerTestApi();
            _error = new Mock<IError>();


            _cacheRepositoryMock = new Mock<ICacheRepository>();
            _mockApi = new Mock<IMockApi>();

            _repository = new Repository( cacheRepository: _cacheRepositoryMock.Object, error: _error.Object,
                _mockApi.Object );
        }

        [Fact]
        public async Task GetAllFromCache()
        {

            var listTestEntities = MockApiFaker.FakerTestApi();

            var valueCached = JsonSerializer.Serialize( listTestEntities );

            var operationResult = new OperationResult<string>( result: valueCached );

            _cacheRepositoryMock.Setup( x => x.GetValueFromKeyAsync( _key ) ).Returns( Task.FromResult( operationResult ) );

            var expected = JsonSerializer.Deserialize<IEnumerable<TestEntity>>( valueCached ).Count();
            var actual = await _repository.GetAllAsync();

            Assert.Equal( expected, actual.Count() );
            _mockApi.Verify( x => x.GetAllAsync(), Times.Never );

        }


        [Fact]
        public async Task GetAllFromApi()
        {
            var listTestEntities = MockApiFaker.FakerTestApi();

            _cacheRepositoryMock.Setup( x => x.GetValueFromKeyAsync( _key ) ).Returns( Task.FromResult( new OperationResult<string>( message: "Erro ao ler dados" ) ) );

            var resultFromApi = new OperationResult<IEnumerable<TestEntity>>( listTestEntities );

            _mockApi.Setup( x => x.GetAllAsync() ).Returns( Task.FromResult( resultFromApi ) );



            var expected = resultFromApi.Result.Count();
            var actual = await _repository.GetAllAsync();
            Assert.Equal( expected, actual.Count() );

        }


        [Fact]
        public async Task GetAllFromApiWithErroInRedis()
        {
            var exception = new Exception( "Erro in Redis" );

            _cacheRepositoryMock.Setup( x => x.GetValueFromKeyAsync( _key ) ).Throws( exception );

            var expected = new List<TestEntity>().Count();
            var actual = await _repository.GetAllAsync();
            Assert.Equal( expected, actual.Count() );
            _mockApi.Verify( x => x.GetAllAsync(), Times.Never );

        }


        [Fact]
        public async Task GetAllFromApiWithErroInApi()
        {
            var exception = new Exception( "Erro in Redis" );

            var listTestEntities = MockApiFaker.FakerTestApi();

            _cacheRepositoryMock.Setup( x => x.GetValueFromKeyAsync( _key ) ).Returns( Task.FromResult( new OperationResult<string>( message: "Erro ao ler dados" ) ) );


            _mockApi.Setup( x => x.GetAllAsync() ).Throws( exception );


            var expected = new List<TestEntity>().Count();
            var actual = await _repository.GetAllAsync();
            Assert.Equal( expected, actual.Count() );

        }
    }
}
