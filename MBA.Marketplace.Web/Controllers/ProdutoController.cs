using AutoMapper;
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
        private readonly IMapper _mapper;
        public ProdutoController(IHttpClientFactory httpClientFactory, ILogger<ProdutoController> logger, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mapper = mapper;
        }
        private async Task<SelectList>  BuscarCategorias()
        {
            var categorias = new SelectList(Enumerable.Empty<SelectListItem>());
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.GetAsync("https://localhost:7053/api/categorias");
            if (response.IsSuccessStatusCode)
            {
                var itens = await response.Content.ReadFromJsonAsync<List<CategoriaViewModel>>();
                categorias = new SelectList(itens, "Id", "Nome");
            }
            return categorias;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
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
            ViewBag.Categorias = await BuscarCategorias();
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
            var client = HttpClientExtensions.CriarRequest(_httpClientFactory, HttpContext);
            var response = await client.GetAsync($"https://localhost:7053/api/produtos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.RegistroNaoEncontrado = true;
                return View();
            }

            ViewBag.Categorias = await BuscarCategorias();
            var produto = await response.Content.ReadFromJsonAsync<ProdutoViewModel>();
            var model = _mapper.Map<ProdutoFormViewModel>(produto);
            return View(model);
        }
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, ProdutoFormViewModel model)
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
            var response = await client.DeleteAsync($"https://localhost:7053/api/produtos/{id}");

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest("Erro ao excluir produto.");
        }
    }
}
