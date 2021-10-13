﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace vector_unitech_application.AutoMapper
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup( this IServiceCollection services )
        {
            if ( services == null ) throw new ArgumentNullException( nameof( services ) );

            services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies() );

            var mappingConfig = RegisterMappings();
            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton( mapper );


        }

        private static MapperConfiguration RegisterMappings()
        {
            var mapper = new MapperConfiguration( cfg =>
            {
                cfg.AddProfile( new DomainToViewModelMappingProfile() );
            } );




            return mapper;
        }
    }
}