using AspNetAuth.Shared.Models;

namespace AspNetAuth.Shared.Classes.Response
{
    public class UserDto
    {
        public UserDto()
        {
        }

        public UserDto(User user)
        {
            Id = user.Id;
            DisplayName = user.DisplayName;
            Email = user.Email;
            Role = user.Role;
            Active = user.Active;
        }

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
    }
}