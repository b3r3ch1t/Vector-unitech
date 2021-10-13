using AutoMapper;
using vector_unitech_application.Models;
using vector_unitech_core.ValueObjects;

namespace vector_unitech_application.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {

        public DomainToViewModelMappingProfile()
        {
            #region Email ==> EmailModel
            CreateMap<Email, EmailModel>()


                .ForMember( d => d.Endereco, o => o.MapFrom( s => s.ToString() ) )

                ;

            #endregion
        }


    }
}
