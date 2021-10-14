namespace Vector_unitech_api.Security.Configurations
{
    public class TokenConfigurations
    {
        public string SecretJWTKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Minutes { get; set; }
    }
}