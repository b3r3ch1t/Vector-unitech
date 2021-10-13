using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vector_unitech_application.Models;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;

namespace vector_unitech_application.AppServices
{
    public class AppService : IAppService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IError _error;
        public AppService( IRepository repository, IMapper mapper, IError error )
        {
            _repository = repository;
            _mapper = mapper;
            _error = error;
        }


        public async Task<OperationResult<IEnumerable<string>>> GetAllEmailsAsync()
        {

            try
            {
                var response = await _repository.GetAllEmailsAsync();

                return new OperationResult<IEnumerable<string>>( response.Select( x => x.Endereco ) );
            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new OperationResult<IEnumerable<string>>( message: "Erro ao retornar os dados" );
        }

        public async Task<OperationResult<IEnumerable<GroupedModel>>> GetNamesGroupedByHourAsync()
        {
            var response = await _repository.GetNamesGroupedByHourAsync();

            try
            {
                var result = _mapper.Map<IEnumerable<GroupedModel>>( response );

                return new OperationResult<IEnumerable<GroupedModel>>( result );
            }
            catch ( Exception e )
            {
                _error.Error( e );
            }

            return new OperationResult<IEnumerable<GroupedModel>>( message: "Erro ao retornar os dados" );
        }

        public void Dispose()
        {
            _repository?.Dispose();
            _error?.Dispose();

            GC.SuppressFinalize( this );

        }
    }
}
