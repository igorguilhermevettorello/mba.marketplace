using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Models;
using MBA.Marketplace.Data.Repositories.Interfaces;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MBA.Marketplace.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository<ApplicationUser> _userRepository;

        public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserRepository<ApplicationUser> userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Token, IEnumerable<string> Errors)> LoginAsync(LoginDto dto)
        {
            var usuario = await _userRepository.FindByEmailAsync(dto.Email);
            var checkPassword = !await _userRepository.CheckPasswordAsync(usuario, dto.Senha);
            if (usuario == null || !checkPassword)
            {
                return (false, null, new[] { "E-mail ou senha inválidos." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (true, tokenString, Array.Empty<string>());
        }
    }
}
