using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Data.Services;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MBA.Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContaService _contaService;

        public LoginController(ILogger<HomeController> logger, IContaService contaService)
        {
            _logger = logger;
            _contaService = contaService; 
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _contaService.LoginAsync(new LoginDto
            {
                Email = model.Email,
                Senha = model.Senha
            });
            
            if (!response.Success) 
            {
                if (response.Errors.Any())
                {
                    foreach (var mensagem in response.Errors)
                    {
                        ModelState.AddModelError("Senha", mensagem);
                    }
                }
                else
                {
                    ModelState.AddModelError("Senha", "Login inválido.");
                }

                return View(model);
            }

            Response.Cookies.Append("AccessToken", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return RedirectToAction("Index", "Home");
        }

    }
}
