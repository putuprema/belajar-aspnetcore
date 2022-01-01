using System.Net.Http;
using System.Threading.Tasks;
using AspNetAuth.Shared.Models;
using AspNetAuth.WebApp.Interfaces;
using AspNetAuth.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.WebApp.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            return View(new UsersListViewModel
            {
                Users = users
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUserStatus(string id, [FromQuery] bool active)
        {
            try
            {
                await _userService.ChangeUserActiveStatus(id, active);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                return View("Index", new UsersListViewModel
                {
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}