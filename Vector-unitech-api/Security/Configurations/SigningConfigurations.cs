using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Vector_unitech_api.Security.Configurations
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations( TokenConfigurations tokenConfigurations )
        {
            Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes( tokenConfigurations.SecretJWTKey ) );

            SigningCredentials = new(
                Key, SecurityAlgorithms.HmacSha256Signature );
        }
    }
}