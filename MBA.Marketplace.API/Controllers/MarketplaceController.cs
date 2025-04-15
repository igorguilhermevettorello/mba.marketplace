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

        public MarketplaceController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }
        [HttpGet("produtos")]
        [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var produtos = await _produtoService.ListarAllAsync();
            return Ok(produtos);
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
