using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/marketplace")]
    public class MarketplaceController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;

        public MarketplaceController(IProdutoService produtoService, ICategoriaService categoriaService)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;   
        }
        [HttpGet("produtos")]
        [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _produtoService.ListarAllAsync();
            return Ok(produtos);
        }
        [HttpGet("categorias")]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarCategorias()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }
        [HttpGet("produtos/categoria/{categoriaId:guid}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarProdutosPorCategoria(Guid categoriaId)
        {
            var produtos = await _produtoService.ListarProdutosPorCategoriaAsync(categoriaId);
            return Ok(produtos);
        }
    }
}
