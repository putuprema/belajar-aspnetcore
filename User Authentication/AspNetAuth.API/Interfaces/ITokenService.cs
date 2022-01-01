using AspNetAuth.Shared.Models;

namespace AspNetAuth.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}