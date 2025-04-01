using MBA.Marketplace.Web.Filters;
using MBA.Marketplace.Web.Helpers;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MBA.Marketplace.Web.Controllers
{
    [Route("categoria")]
    [Autorizado]
    public class CategoriaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.AdicionarToken(HttpContext);
            var response = await client.GetAsync("https://localhost:7053/api/categorias");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Erro = "Erro ao carregar categorias.";
                return View(new List<CategoriaViewModel>());
            }

            var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaViewModel>>();
            return View(categorias);
        }

        [HttpGet("criar")]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Criar(CategoriaFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            client.AdicionarToken(HttpContext);
            var response = await client.PostAsJsonAsync("https://localhost:7053/api/categorias", model);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.RegistroSucesso = true;
                return View();
            }
            else
            {
                // tenta ler os erros do ModelState da API
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    ModelState.AddModelError(string.Empty, "Erro desconhecido ao registrar.");
                    ViewBag.RegistroErro = true;
                    return View(model);
                }

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
                            ModelState.AddModelError(field.Key, errorMsg);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro desconhecido ao registrar.");
                    ViewBag.RegistroErro = true;
                }

                return View(model);
            }
        }
    }
}
