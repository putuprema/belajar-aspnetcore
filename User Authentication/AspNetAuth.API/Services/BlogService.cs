using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetAuth.API.Data;
using AspNetAuth.API.Interfaces;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AspNetAuth.API.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateBlogPost(CreateBlogPostRequest request)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blog = new BlogPost
            {
                UserId = currentUserId,
                Title = request.Title,
                Body = request.Body
            };

            _dbContext.BlogPosts.Add(blog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBlog(string blogId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blog = await _dbContext.BlogPosts
                .Where(x => x.UserId == currentUserId && x.Id == blogId)
                .FirstOrDefaultAsync();

            if (blog == null)
                throw new Exception("Blog not found");

            _dbContext.BlogPosts.Remove(blog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BlogPostDto>> GetAllBlogPosts()
        {
            var blogs = await _dbContext.BlogPosts
                .Include(x => x.User)
                .ToListAsync();

            return blogs.Select(x => new BlogPostDto(x)).ToList();
        }

        public async Task<List<BlogPostDto>> GetCurrentUserBlogPosts()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blogs = await _dbContext.BlogPosts.Where(x => x.UserId == currentUserId).ToListAsync();
            return blogs.Select(x => new BlogPostDto(x)).ToList();
        }
    }
}