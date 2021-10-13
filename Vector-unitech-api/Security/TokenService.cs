using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using Vector_unitech_api.Security.Configurations;
using vector_unitech_core.Utils;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Vector_unitech_api.Security
{
    public class TokenService
    {

        private readonly IDistributedCache _cache;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly SigningConfigurations _signingConfigurations;

        public TokenService( IDistributedCache cache, TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations )
        {
            _cache = cache;
            _tokenConfigurations = tokenConfigurations;
            _signingConfigurations = signingConfigurations;
        }


        public async Task<OperationResult<Token>> GenerateTokenAsync( User user )
        {

            var listClaims = new List<Claim>();

            if ( user.Role == "Admin" )
            {
                listClaims.Add( new Claim( ClaimTypes.Authentication, "Admin" ) );

            }
            var identity = new ClaimsIdentity(
                new GenericIdentity( user.Id.ToString(), "Login" ),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
                }
            );


            foreach ( var claim in listClaims )
            {
                identity.AddClaim( claim );

            }

            var dataCriacao = DateTime.Now;
            var dataExpiracao = dataCriacao +
                                TimeSpan.FromMinutes( _tokenConfigurations.Minutes );



            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao,
                Claims = new Dictionary<string, object>()
            };

            var securityToken = handler.CreateToken( tokenDescriptor );

            var token = handler.WriteToken( securityToken );
            var resultado = new Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString( "yyyy-MM-dd HH:mm:ss" ),
                Expiration = dataExpiracao.ToString( "yyyy-MM-dd HH:mm:ss" ),
                AccessToken = token,
                RefreshToken = Guid.NewGuid().ToString().Replace( "-", string.Empty ),
                Message = "OK"
            };

            // Armazena o refresh token em cache através do Redis 
            var refreshTokenData = new RefreshTokenDataModel
            {
                RefreshToken = resultado.RefreshToken,
                UserID = user.Id
            };


            // Calcula o tempo máximo de validade do refresh token
            // (o mesmo será invalidado automaticamente pelo Redis)
            var finalExpiration =
                TimeSpan.FromSeconds( _tokenConfigurations.FinalExpiration );

            var opcoesCache =
                new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration( finalExpiration );
            try
            {
                await _cache.SetStringAsync( resultado.RefreshToken,
                    JsonSerializer.Serialize( refreshTokenData ),
                    opcoesCache );
            }
            catch ( Exception e )
            {

                Console.WriteLine( e.Message );

            }


            return new OperationResult<Token>( resultado );
        }
    }
}
