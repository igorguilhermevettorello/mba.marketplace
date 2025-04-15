using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Models;
using MBA.Marketplace.Data.Repositories;
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
    public class ContaService : IContaService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IVendedorRepository _vendedorRepository;
        private string[] ErrorPassowrd = { "PasswordTooShort", "PasswordRequiresNonAlphanumeric", "PasswordRequiresLower", "PasswordRequiresUpper", "PasswordRequiresDigit" };
        private string[] ErrorEmail = { "DuplicateUserName" };
        public ContaService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IVendedorRepository vendedorRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _vendedorRepository = vendedorRepository;
        }
        public async Task<(bool Success, string Token, IEnumerable<string> Errors, string UserId)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Senha))
            {
                return (false, null, new[] { "E-mail ou senha inválidos." }, null);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
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

            return (true, tokenString, Array.Empty<string>(), user.Id);
        }

        public async Task<RetornoRegistrarUsuarioDto> RegisterAsync(RegistrarUsuarioDto dto)
        {
            var _retorno = new RetornoRegistrarUsuarioDto();

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (!result.Succeeded)
            {
                _retorno.Status = false;
                foreach (var error in result.Errors)
                {
                    var field = "Identity";
                    if (ErrorEmail.Contains(error.Code))
                        field = "Email";
                    else if (ErrorPassowrd.Contains(error.Code))
                        field = "Senha";

                    _retorno.Error.Add(new KeyValuePair<string, string>(field, error.Description));
                }

                return _retorno;
            }

            var vendedor = new Vendedor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                UsuarioId = user.Id,
                CreatedAt = DateTime.Now
            };

            _retorno.Status = await _vendedorRepository.CriarAsync(vendedor);
            return _retorno;
        }
    }
}
