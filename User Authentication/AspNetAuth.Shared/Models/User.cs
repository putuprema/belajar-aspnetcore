using System;

namespace AspNetAuth.Shared.Models
{
    public static class Role
    {
        public const string User = "User";
        public const string Admin = "Admin";
    }

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; } = true;
    }
}