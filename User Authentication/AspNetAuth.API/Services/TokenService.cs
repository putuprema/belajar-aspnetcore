using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetAuth.API.Interfaces;
using AspNetAuth.Shared.Classes;
using AspNetAuth.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace AspNetAuth.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly SymmetricSecurityKey _signingKey;

        public TokenService(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
        }

        public string GenerateToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _jwtConfig.ValidIssuer,
                expires: DateTime.Now.AddSeconds(_jwtConfig.Lifetime),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}