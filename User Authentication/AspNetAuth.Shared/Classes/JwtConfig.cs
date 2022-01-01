namespace AspNetAuth.Shared.Classes
{
    public class JwtConfig
    {
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public double Lifetime { get; set; }
    }
}