using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Vector_unitech_api
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {

            services.AddControllers();


            //Swagger
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
