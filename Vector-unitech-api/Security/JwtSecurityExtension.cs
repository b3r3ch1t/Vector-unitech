using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using Vector_unitech_api.Security.Configurations;

namespace Vector_unitech_api.Security
{
    public static class JwtSecurityExtension
    {
        public static IServiceCollection AddJwtSecurity(
            this IServiceCollection services,
            TokenConfigurations tokenConfigurations )
        {
            services.AddSingleton( tokenConfigurations );

            services.AddScoped<TokenService>();


            var signingConfigurations =
                new SigningConfigurations( tokenConfigurations );


            services.AddSingleton( signingConfigurations );

            services.AddAuthentication( authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            } ).AddJwtBearer( bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            } );


            services.AddAuthorization( options =>
            {
                options.AddPolicy( "Admin", policy => policy.RequireClaim( ClaimTypes.Authentication, "Admin" ) );

            } );

            return services;
        }
    }
}
