using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Data.Attributes;
using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MBA.Marketplace.Data.Repositories.Interfaces;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IVendedorRepository _vendedorRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoService _produtoService;
        private readonly IConfiguration _config;
        public ProdutoController(IVendedorRepository vendedorRepository, ICategoriaRepository categoriaRepository, IProdutoService produtoService, IConfiguration config)
        {
            _vendedorRepository = vendedorRepository;
            _categoriaRepository = categoriaRepository;
            _produtoService = produtoService;
            _config = config;
        }
        private async Task<Vendedor> BuscarVendedorLogado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vendedor = await _vendedorRepository.ObterPorUsuarioIdAsync(userId);

            if (vendedor == null)
                throw new UnauthorizedAccessException("Usuário não é um vendedor válido.");

            return vendedor;
        }
        private async Task<Categoria?> BuscarCategoria(Guid? id)
        {
            return await _categoriaRepository.ObterPorIdAsync(id);
        }
        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
        [HttpPost]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromForm] ProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendedor = await BuscarVendedorLogado();

            var categoria = await BuscarCategoria(dto.CategoriaId);
            if (categoria == null) 
                return BadRequest(new { mensagem = "Categoria não é válida." });

            var produto = await _produtoService.CriarAsync(dto, vendedor);
            return CreatedAtAction(null, new { id = produto.Id }, produto);
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var vendedor = await BuscarVendedorLogado();
            var produtos = await _produtoService.ListarAsync(vendedor);
            return Ok(produtos);
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var vendedor = await BuscarVendedorLogado();
            var produto = await _produtoService.ObterPorIdAsync(id, vendedor);
            if (produto == null) return NotFound();

            return Ok(produto);
        }
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] ProdutoEditDto dto, IFormFile? imagem)
        {
            ModelState.Remove("imagem");
            if (imagem != null)
            {
                var validador = new ImagemAttribute();
                var resultado = validador.GetValidationResult(imagem, new ValidationContext(imagem));
                if (resultado != ValidationResult.Success)
                    ModelState.AddModelError("Imagem", resultado.ErrorMessage);
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendedor = await BuscarVendedorLogado();

            var categoria = await BuscarCategoria(dto.CategoriaId);
            if (categoria == null) return BadRequest(new { mensagem = "Categoria não é válida." });

            var sucesso = await _produtoService.AtualizarAsync(id, dto, vendedor, imagem);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid id)
        {
            var vendedor = await BuscarVendedorLogado();

            var sucesso = await _produtoService.RemoverAsync(id, vendedor);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
        [HttpGet("imagem/{nome}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ObterImagem(string nome)
        {
            var pasta = _config["SharedFiles:ImagensPath"];
            string caminhoPasta = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, pasta);
            var caminho = Path.Combine(caminhoPasta, nome);

            if (!System.IO.File.Exists(caminho))
                return NotFound("Imagem não encontrada.");

            var contentType = GetContentType(caminho);

            var imagem = System.IO.File.OpenRead(caminho);
            return File(imagem, contentType);
        }
    }
}
