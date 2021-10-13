using AutoMapper;
using System.Linq;
using vector_unitech_application.Models;
using vector_unitech_core.Entities;
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

            #region GroupedByHour ==>GroupedByHourModel

            CreateMap<GroupedByHour, GroupedByHourModel>()
                .ForMember( d => d.Data, o => o.MapFrom( s => s.CreatedAt ) )
                .ForMember( d => d.Nomes, o => o.MapFrom( s => s.ListEntity.Select( x => x.Name ) ) )

                ;
            #endregion
        }


    }
}
