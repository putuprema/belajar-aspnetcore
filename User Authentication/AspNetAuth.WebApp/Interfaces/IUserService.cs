using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;

namespace AspNetAuth.WebApp.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginUserRequest request);
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetCurrentUserProfile();
        Task ChangeUserActiveStatus(string userId, bool active);
    }
}