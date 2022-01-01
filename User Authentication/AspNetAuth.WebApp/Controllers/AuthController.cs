using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.WebApp.Constants;
using AspNetAuth.WebApp.Interfaces;
using AspNetAuth.WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtConfig _jwtConfig;

        public AuthController(IUserService userService, JwtConfig jwtConfig)
        {
            _userService = userService;
            _jwtConfig = jwtConfig;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginResult = await _userService.Login(new LoginUserRequest
                    {
                        Email = viewModel.Email,
                        Password = viewModel.Password,
                        Role = viewModel.Role
                    });

                    // Save JWT token to cookie
                    Response.Cookies.Append(Defaults.AccessTokenCookieKey, loginResult.AccessToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddSeconds(_jwtConfig.Lifetime),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });

                    return RedirectToAction("Index", "Home");
                }
                catch (HttpRequestException ex)
                {
                    viewModel.ErrorMessage = ex.Message;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove JWT token from cookie
            Response.Cookies.Delete(Defaults.AccessTokenCookieKey);

            return RedirectToAction("Index", "Home");
        }
    }
}