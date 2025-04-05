using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/marketplace")]
    public class MarketplaceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProdutoService _produtoService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public MarketplaceController(ApplicationDbContext context, IProdutoService produtoService, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _produtoService = produtoService;
            _env = env;
            _config = config;
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
