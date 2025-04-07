using AutoMapper;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MBA.Marketplace.Web.Controllers
{
    [Route("produto")]
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IProdutoService _produtoService;
        private readonly IVendedorService _vendedorService;
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMapper _mapper;
        public ProdutoController(ICategoriaService categoriaService, IProdutoService produtoService, IVendedorService vendedorService, ILogger<ProdutoController> logger, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _produtoService = produtoService;
            _vendedorService = vendedorService;
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
            ViewBag.Categorias = await BuscarCategorias();
            var vendedor = await BuscarVendedorLogado();
            var produto = await _produtoService.ObterPorIdAsync(id, vendedor);

            if (produto == null)
            {
                ViewBag.RegistroNaoEncontrado = true;
                return View(new ProdutoFormViewModel());
            }

            var model = _mapper.Map<ProdutoFormViewModel>(produto);
            return View(model);
        }
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Editar(Guid id, ProdutoFormViewModel model)
        {
            try
            {
                ViewBag.Categorias = await BuscarCategorias();

                ModelState.Remove("Imagem");
                ModelState.Remove("Src");

                if (!ModelState.IsValid)
                    return View(model);

                var vendedor = await BuscarVendedorLogado();
                var response = await _produtoService.AtualizarAsync(id, new ProdutoEditDto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Preco = model.Preco,
                    Estoque = model.Estoque,
                    CategoriaId = model.CategoriaId
                }, vendedor, model.Imagem);

                if (response)
                {
                    ViewBag.RegistroSucesso = true;
                    return View(new ProdutoFormViewModel());
                }
                else
                {
                    ViewBag.RegistroErro = true;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao registrar.");
                ViewBag.RegistroErro = true;
                return View(model);
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
        [HttpGet("detalhe/{id:Guid}")]
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var produto = await _produtoService.PublicObterPorIdAsync(id);
            if (produto == null) return NotFound();

            var viewModel = _mapper.Map<ProdutoViewModel>(produto); // ou manual
            return View(viewModel);
        }
    }
}
