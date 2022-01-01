using System.Net.Http;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Models;
using AspNetAuth.WebApp.Interfaces;
using AspNetAuth.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.WebApp.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
            
        }
        
        [Authorize(Roles = Role.User)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Role.Admin)]
        public IActionResult All()
        {
            return View();
        }

        [Authorize(Roles = Role.User)]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.User)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveBlog(NewBlogViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _blogService.CreateBlogPost(new CreateBlogPostRequest
                    {
                        Title = viewModel.Title,
                        Body = viewModel.Body
                    });

                    return RedirectToAction("Index");
                }
                catch (HttpRequestException ex)
                {
                    viewModel.ErrorMessage = ex.Message;
                }
            }

            return View("New", viewModel);
        }
    }
}