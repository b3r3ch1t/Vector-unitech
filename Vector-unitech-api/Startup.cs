using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using vector_unitech.log;
using Vector_unitech_api.Extensions;
using vector_unitech_application.AppServices;
using vector_unitech_application.AutoMapper;
using vector_unitech_core.Interfaces;
using vector_unitech_core.Utils;
using vector_unitech_data;

namespace Vector_unitech_api
{
    public class Startup
    {
        private IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; }

        public Startup( IWebHostEnvironment env )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath );

            if ( env.IsStaging() )
            {
                builder.AddEnvironmentVariables();
            }

            if ( env.IsDevelopment() )
            {
                builder.AddUserSecrets<Startup>();
            }


            if ( env.IsProduction() )
            {
                builder.AddJsonFile( "appsettings.json", true, true );
            }


            Configuration = builder.Build();

            WebHostEnvironment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {

            AppSettings.RedisServer = Configuration.GetConnectionString( "ConexaoRedis" );

            services.AddControllers();

            #region Swagger
            services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc( "v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Vector Unitech",
                    Description = "Teste - Vector Unitech",

                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri( "https://github.com/brunohbrito/RESTFul.API-Example/blob/master/LICENSE" )
                    },

                } );


            } );

            #endregion

            #region Erro Centralizado

            services.AddGlobalExceptionHandlerMiddleware();

            #endregion

            #region  Log

            var sentryDsn = Configuration[ "SentryDsn" ];
            if ( !string.IsNullOrEmpty( sentryDsn ) )
            {
                Serilog.Core.Logger log = null;


                if ( WebHostEnvironment.IsStaging() || WebHostEnvironment.IsProduction() )
                {
                    log = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.Sentry( sentryDsn )
                        .CreateLogger();
                }

                if ( WebHostEnvironment.IsDevelopment() )
                {
                    log = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .CreateLogger();
                }


                services.AddScoped<ILogger>( x => log );

                services.AddSingleton<IError, ErroSentry>( x => new ErroSentry( sentryDsn ) );

            }



            #endregion

            #region CacheRedis

            services.AddDistributedRedisCache( options =>
            {
                options.Configuration = AppSettings.RedisServer;
                options.InstanceName = "vector-unitech-";
            } );


            services.AddSingleton<ICacheRepository, CacheRepository>();

            #endregion

            #region AutoMapper

            services.AddAutoMapperSetup();

            #endregion

            services.AddScoped<IAppService, AppService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseHttpsRedirection();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
            } );


            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers();
             } );


            app.UseSwaggerUI( c =>
            {
                c.SwaggerEndpoint( "/swagger/v1/swagger.json", "My API V1" );
                c.RoutePrefix = string.Empty;
            } );
        }
    }
}
