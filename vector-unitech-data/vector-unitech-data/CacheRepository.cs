using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;

namespace vector_unitech_data
{
    public class CacheRepository : ICacheRepository
    {
        private readonly ConnectionMultiplexer _conexao;
        private readonly IDatabase _database;
        private readonly IError _error;

        public CacheRepository( IError error )
        {
            _error = error;
            _conexao = ConnectionMultiplexer.Connect( AppSettings.RedisServer );
            _database = _conexao.GetDatabase();

        }

        public async Task<OperationResult<string>> GetValueFromKeyAsync( string key )
        {
            try
            {
                var response = await _database.StringGetAsync( key );

                if ( response.IsNullOrEmpty )
                    return new OperationResult<string>( message: "valor não encontrado no cache" );
                return new OperationResult<string>()
                {
                    Error = false,
                    Result = response
                };
            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new OperationResult<string>( message: "Erro ao recuperar os dados" );

        }


        public async Task<OperationResult<bool>> SetValueToKey<T>( string key, T value, DateTime expires )
        {
            try
            {
                var expiryTimeSpan = expires.Subtract( DateTime.Now );

                var serialized = JsonSerializer.Serialize( value );

                var response = await _database.StringSetAsync( key: key, value: serialized, expiryTimeSpan );

                return new OperationResult<bool>( response );
            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new OperationResult<bool>( message: "Erro ao salvar os dados" );

        }
        public void Dispose()
        {
            _conexao?.Dispose();
            GC.SuppressFinalize( this );
        }
    }
}
