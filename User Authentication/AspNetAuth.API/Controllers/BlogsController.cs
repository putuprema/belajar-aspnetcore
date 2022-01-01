using System;
using System.Threading.Tasks;
using AspNetAuth.API.Interfaces;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("My")] //api/Blogs/My
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> GetCurrentUserBlogPosts()
        {
            try
            {
                var result = await _blogService.GetCurrentUserBlogPosts();
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            try
            {
                var result = await _blogService.GetAllBlogPosts();
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequest request)
        {
            try
            {
                await _blogService.CreateBlogPost(request);
                return Ok();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpDelete("{blogId}")]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> DeleteBlogPost(string blogId)
        {
            try
            {
                await _blogService.DeleteBlog(blogId);
                return Ok();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }
    }
}