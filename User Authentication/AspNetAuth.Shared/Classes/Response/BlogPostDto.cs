using AspNetAuth.Shared.Models;

namespace AspNetAuth.Shared.Classes.Response
{
    public class BlogPostDto
    {
        public BlogPostDto()
        {
        }

        public BlogPostDto(BlogPost blogPost)
        {
            Id = blogPost.Id;

            if (blogPost.User != null) User = new UserDto(blogPost.User);

            Title = blogPost.Title;
            Body = blogPost.Body;
        }

        public string Id { get; set; }
        public UserDto User { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}