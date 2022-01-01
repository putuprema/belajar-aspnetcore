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
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public UserService(AppDbContext dbContext, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users.Select(u => new UserDto(u)).ToList();
        }

        public async Task<UserDto> GetCurrentUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (user == null)
                throw new Exception("User not found");

            return new UserDto(user);
        }

        public async Task ChangeUserActiveStatus(string userId, bool active)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new Exception("User not found");

            user.Active = active;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<LoginResponse> Login(LoginUserRequest request)
        {
            var user = await _dbContext.Users
                .Where(x => x.Email == request.Email && x.Role == request.Role && x.Active == true)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new Exception("Wrong password");

            return new LoginResponse
            {
                AccessToken = _tokenService.GenerateToken(user),
                User = new UserDto(user)
            };
        }

        public async Task RegisterUser(RegisterUserRequest request)
        {
            var existingUser = await _dbContext.Users.Where(x => x.Email == request.Email && x.Role == request.Role)
                .FirstOrDefaultAsync();
            if (existingUser != null)
                throw new Exception("User already exist");

            var user = new User
            {
                DisplayName = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}