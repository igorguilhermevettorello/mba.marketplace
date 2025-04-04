using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Models;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MBA.Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContaService _contaService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(ILogger<HomeController> logger, IContaService contaService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _contaService = contaService; 
            _signInManager = signInManager;
            _userManager = userManager;
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

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, false);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError("Senha", "Login inválido.");
                return View(model);
            }
            //// Cria as claims
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, response.UserId),
            //    new Claim(ClaimTypes.Name, model.Email),
            //    new Claim(ClaimTypes.Email, model.Email)
            //};

            //var identity = new ClaimsIdentity(claims, "Cookies");

            //var authProperties = new AuthenticationProperties
            //{
            //    IsPersistent = true,
            //    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            //};

            //await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity), authProperties);
            

            //return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            //    ? Redirect(returnUrl)
            //    : RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Home");

            //var claimsIdentity = new ClaimsIdentity(response.claims, "Cookies");

            //var authProperties = new AuthenticationProperties
            //{
            //    IsPersistent = true,
            //    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            //};

            //await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

            //return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
