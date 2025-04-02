using MBA.Marketplace.Web.Filters;
using MBA.Marketplace.Web.Helpers;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MBA.Marketplace.Web.Controllers
{
    [Route("produto")]
    [Autorizado]
    public class ProdutoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProdutoController> _logger;
        public ProdutoController(IHttpClientFactory httpClientFactory, ILogger<ProdutoController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.AdicionarToken(HttpContext);
            var response = await client.GetAsync("https://localhost:7053/api/produtos");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Erro = "Erro ao carregar produtos.";
                return View(new List<ProdutoViewModel>());
            }

            var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoViewModel>>();
            return View(produtos);
        }

        [HttpGet("criar")]
        public async Task<IActionResult> Criar()
        {
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.GetAsync("https://localhost:7053/api/categorias");

            if (response.IsSuccessStatusCode)
            {
                var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaViewModel>>();
                ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");
            }
            else
            {
                ViewBag.Categorias = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            return View();
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Criar(ProdutoFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(model.Nome), "Nome");
                form.Add(new StringContent(model.Descricao), "Descricao");
                form.Add(new StringContent(model.Preco.ToString()), "Preco");
                form.Add(new StringContent(model.Estoque.ToString()), "Estoque");
                form.Add(new StringContent(model.CategoriaId.ToString()), "CategoriaId");
                if (model.Imagem != null && model.Imagem.Length > 0)
                {
                    var streamContent = new StreamContent(model.Imagem.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.Imagem.ContentType);
                    form.Add(streamContent, "Imagem", model.Imagem.FileName);
                }

                var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
                var response = await client.PostAsync("https://localhost:7053/api/produtos", form);

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

                    try
                    {
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
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "Erro desconhecido ao registrar.");
                        ModelState.AddModelError(string.Empty, "Erro desconhecido ao registrar.");
                        ViewBag.RegistroErro = true;
                    }



                    return View(model);
                }
            }
        }

        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id)
        {
            var token = HttpContext.Request.Cookies["AccessToken"];
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

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

            var token = HttpContext.Request.Cookies["AccessToken"];
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PutAsJsonAsync($"https://localhost:7053/api/categorias/{model.Id}", model);

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
            var token = HttpContext.Request.Cookies["AccessToken"];
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.DeleteAsync($"https://localhost:7053/api/categorias/{id}");

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest("Erro ao excluir categoria.");
        }
    }
}
