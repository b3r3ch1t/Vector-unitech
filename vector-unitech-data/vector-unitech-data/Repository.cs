using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vector_unitech_core.Entities;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;
using vector_unitech_core.ValueObjects;

namespace vector_unitech_data
{
    public class Repository : IRepository
    {
        private readonly IDatabase _dbRedis;

        public Repository()
        {
            var conexaoRedis = ConnectionMultiplexer.Connect( AppSettings.RedisServer );
            _dbRedis = conexaoRedis.GetDatabase();
        }
        public Task<IEnumerable<Email>> GetAllEmailsAsync()
        {
            throw new NotImplementedException();
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
