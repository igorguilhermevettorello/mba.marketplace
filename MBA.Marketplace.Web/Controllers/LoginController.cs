using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public LoginController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Simulação de login — aqui você chamaria sua API
            if (model.Email == "admin@email.com" && model.Senha == "123456")
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        public IActionResult EsqueciSenha() => View(); // ainda será criada
        public IActionResult Registrar() => RedirectToAction("Registrar", "Conta");
    }
}
