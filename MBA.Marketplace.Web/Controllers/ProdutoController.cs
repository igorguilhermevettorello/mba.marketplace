using AutoMapper;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Services;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.Filters;
using MBA.Marketplace.Web.Helpers;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MBA.Marketplace.Web.Controllers
{
    [Route("produto")]
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IProdutoService _produtoService;
        private readonly IVendedorService _vendedorService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMapper _mapper;
        public ProdutoController(ICategoriaService categoriaService, IProdutoService produtoService, IVendedorService vendedorService, IHttpClientFactory httpClientFactory, ILogger<ProdutoController> logger, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _produtoService = produtoService;
            _vendedorService = vendedorService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mapper = mapper;
        }
        private async Task<SelectList> BuscarCategorias()
        {
            var select = new SelectList(Enumerable.Empty<SelectListItem>());
            var categorias = await _categoriaService.ListarAsync();
            if (categorias.Any())
            {
                var itens = _mapper.Map<List<CategoriaViewModel>>(categorias);
                select = new SelectList(itens, "Id", "Nome");
            }
            return select;
        }
        private async Task<Vendedor> BuscarVendedorLogado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vendedor = await _vendedorService.ObterPorIdAsync(userId);   
            if (vendedor == null)
                throw new UnauthorizedAccessException("Usuário não é um vendedor válido.");

            return vendedor;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vendedor = await BuscarVendedorLogado();
            var produtos = await _produtoService.ListarAsync(vendedor);
            var model = _mapper.Map<List<ProdutoViewModel>>(produtos);
            return View(model);
        }
        [HttpGet("criar")]
        public async Task<IActionResult> Criar()
        {
            ViewBag.Categorias = await BuscarCategorias();
            return View(new ProdutoFormViewModel());
        }
        [HttpPost("criar")]
        public async Task<IActionResult> Criar(ProdutoFormViewModel model)
        {
            ViewBag.Categorias = await BuscarCategorias();

            ModelState.Remove("Imagem");
            ModelState.Remove("Src");
            if (model.Imagem == null)
                ModelState.AddModelError("Imagem", "A imagem é obrigatória.");
            
            if (!ModelState.IsValid)
                return View(model);

            var vendedor = await BuscarVendedorLogado();

            try
            {
                var _ = await _produtoService.CriarAsync(new ProdutoDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Preco = model.Preco,
                    Estoque = model.Estoque,
                    CategoriaId = model.CategoriaId,
                    Imagem = model.Imagem
                }, vendedor);

                ViewBag.RegistroSucesso = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao registrar.");
                ViewBag.RegistroErro = true;
            }

            return View(model);
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
            ViewBag.Categorias = await BuscarCategorias();

            ModelState.Remove("Imagem");
            ModelState.Remove("Src");
            
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
                var response = await client.PutAsync($"https://localhost:7053/api/produtos/{id}", form);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.RegistroSucesso = true;
                    return View(new ProdutoFormViewModel());
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
        [HttpDelete("deletar/{id:Guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var vendedor = await BuscarVendedorLogado();
            var response = await _produtoService.RemoverAsync(id, vendedor);

            if (response)
                return Ok();

            return BadRequest("Erro ao excluir produto.");
        }
    }
}
