using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MBA.Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public LoginController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory; 
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

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7053/api/conta/login", model);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (errors != null)
                {
                    foreach (var field in errors)
                    {
                        foreach (var errorMsg in field.Value)
                        {
                            ModelState.AddModelError("Senha", errorMsg);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Senha", "Login inválido.");
                }

                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

            Response.Cookies.Append("AccessToken", result.Token, new CookieOptions
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
