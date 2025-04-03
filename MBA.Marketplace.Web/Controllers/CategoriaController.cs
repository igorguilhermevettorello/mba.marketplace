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
        private readonly ILogger<ProdutoController> _logger;
        public CategoriaController(IHttpClientFactory httpClientFactory, ILogger<ProdutoController> logger) 
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
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

            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
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
        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id)
        {
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.GetAsync($"https://localhost:7053/api/categorias/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.RegistroNaoEncontrado = true;
                return View();
            }
            
            var categoria = await response.Content.ReadFromJsonAsync<CategoriaFormViewModel>();
            return View(categoria);
        }
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, CategoriaFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.PutAsJsonAsync($"https://localhost:7053/api/categorias/{id}", model);

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
        [HttpDelete("deletar/{id:Guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.DeleteAsync($"https://localhost:7053/api/categorias/{id}");

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var resultado = JsonSerializer.Deserialize<MensagemErroViewModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var mensagem = resultado?.Mensagem ?? "Erro ao excluir categoria.";

                    return BadRequest(mensagem);
                }
                catch (Exception ex)
                {
                    return BadRequest("Erro ao excluir categoria.");
                }
            }

            return Ok();
        }
    }
}
