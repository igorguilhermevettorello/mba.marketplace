using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Models;
using MBA.Marketplace.Data.Repositories.Interfaces;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class ContaController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVendedorRepository _vendedorRepository;
        private readonly IAccountService _accountService;

        private string[] ErrorPassowrd = { "PasswordTooShort", "PasswordRequiresNonAlphanumeric", "PasswordRequiresLower", "PasswordRequiresUpper", "PasswordRequiresDigit" };
        private string[] ErrorEmail = { "DuplicateUserName" };
        public ContaController(UserManager<ApplicationUser> userManager, IVendedorRepository vendedorRepository, IAccountService accountService)
        {
            _userManager = userManager;
            _vendedorRepository = vendedorRepository;
            _accountService = accountService;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            await _vendedorRepository.CriarAsync(new Vendedor
            {
                Nome = dto.Nome,
                Email = dto.Email,
                UsuarioId = user.Id,
                CreatedAt = DateTime.Now
            });
            
            return Ok(new { message = "Conta criada com sucesso." });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
