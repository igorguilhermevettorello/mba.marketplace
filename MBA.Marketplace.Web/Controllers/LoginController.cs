using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Models;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IContaService _contaService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LoginController(IContaService contaService, SignInManager<ApplicationUser> signInManager)
        {
            _contaService = contaService; 
            _signInManager = signInManager;
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
            
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
