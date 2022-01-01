using System;

namespace AspNetAuth.Shared.Models
{
    public class BlogPost
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}