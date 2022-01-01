using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.Shared.Models;
using AspNetAuth.WebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.WebApp.ApiControllers
{
    [Route("api/Blogs")]
    [ApiController]
    public class BlogsApiController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsApiController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        
        [HttpGet("My")]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> GetCurrentUserBlogPosts()
        {
            try
            {
                // Simulate slow network connection
                await Task.Delay(1000);
                
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
                // Simulate slow network connection
                await Task.Delay(1000);
                
                var result = await _blogService.GetAllBlogPosts();
                return Ok(result);
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
                // Simulate slow network connection
                await Task.Delay(1000);
                
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