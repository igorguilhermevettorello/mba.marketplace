using MBA.Marketplace.Web.Filters;
using MBA.Marketplace.Web.Helpers;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MBA.Marketplace.Web.Controllers
{
    [Autorizado]
    public class CategoriaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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
    }
}
