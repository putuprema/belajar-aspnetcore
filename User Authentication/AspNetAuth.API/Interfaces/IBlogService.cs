using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;

namespace AspNetAuth.API.Interfaces
{
    public interface IBlogService
    {
        Task CreateBlogPost(CreateBlogPostRequest request);
        Task DeleteBlog(string blogId);
        Task<List<BlogPostDto>> GetAllBlogPosts();
        Task<List<BlogPostDto>> GetCurrentUserBlogPosts();
    }
}