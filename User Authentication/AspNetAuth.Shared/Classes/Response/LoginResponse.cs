namespace AspNetAuth.Shared.Classes.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public UserDto User { get; set; }
    }
}