using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Core.Models;
using MBA.Marketplace.Core.Services.Interfaces;
using MBA.Marketplace.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class ContaController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        private string[] ErrorPassowrd = { "PasswordTooShort", "PasswordRequiresNonAlphanumeric", "PasswordRequiresLower", "PasswordRequiresUpper", "PasswordRequiresDigit" };
        private string[] ErrorEmail = { "DuplicateUserName" };
        public ContaController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IAccountService accountService)
        {
            _userManager = userManager;
            _context = context;
            _accountService = accountService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] RegistrarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    var field = "Identity";
                    if (ErrorEmail.Contains(error.Code))
                        field = "Email";
                    else if (ErrorPassowrd.Contains(error.Code))
                        field = "Senha";

                    ModelState.AddModelError(field, error.Description);
                }
                
                return BadRequest(ModelState);
            }

            var vendedor = new Vendedor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                UsuarioId = user.Id,
                CreatedAt = DateTime.Now
            };

            _context.Vendedores.Add(vendedor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Conta criada com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, token, errors) = await _accountService.LoginAsync(dto);

            if (!success)
                return Unauthorized(new { Errors = errors });

            return Ok(new { Token = token });
        }
    }
}
