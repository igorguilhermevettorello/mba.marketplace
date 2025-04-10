using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContaService _contaService;
        public RegistrarController(ILogger<HomeController> logger, IContaService contaService)
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
        public async Task<IActionResult> Index(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _contaService.RegisterAsync(new RegistrarUsuarioDto
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
                ConfirmacaoSenha = model.ConfirmacaoSenha
            });

            if (response.Status)
            {
                ViewBag.RegistroSucesso = true;
                return View();
            }

            if (response.Error.Any())
            {
                foreach (var item in response.Error)
                {
                    if (item.Key == "Identity")
                        ModelState.AddModelError(item.Key == string.Empty ? "" : item.Key, item.Value);
                    else
                        ModelState.AddModelError(item.Key, item.Value);
                }
            }
            else 
            {
                ViewBag.RegistroErro = true;
            }

            return View(model);
        }
    }
}
