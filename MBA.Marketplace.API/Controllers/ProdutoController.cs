using MBA.Marketplace.Data.Services.Interfaces;
using MBA.Marketplace.Core.Attributes;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProdutoService _produtoService;
        private readonly IWebHostEnvironment _env;
        public ProdutoController(ApplicationDbContext context, IProdutoService produtoService, IWebHostEnvironment env)
        {
            _context = context;
            _produtoService = produtoService;
            _env = env;
        }
        private async Task<Vendedor> BuscarVendedorLogado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(v => v.UsuarioId == userId);

            if (vendedor == null)
                throw new UnauthorizedAccessException("Usuário não é um vendedor válido.");

            return vendedor;
        }
        private async Task<Categoria> BuscarCategoria(Guid? id)
        {
            var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                throw new ArgumentException("Categoria não é válido.");

            return categoria;
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
        public async Task<IActionResult> Criar([FromForm] ProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendedor = await BuscarVendedorLogado();
            var categoria = await BuscarCategoria(dto.CategoriaId);
            var produto = await _produtoService.CriarAsync(dto, vendedor);
            return CreatedAtAction(null, new { id = produto.Id }, produto);
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var vendedor = await BuscarVendedorLogado();
            var produtos = await _produtoService.ListarAsync(vendedor);
            return Ok(produtos);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var vendedor = await BuscarVendedorLogado();
            var produto = await _produtoService.ObterPorIdAsync(id, vendedor);
            if (produto == null) return NotFound();

            return Ok(produto);
        }
        [HttpPut("{id:guid}")]
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

            var sucesso = await _produtoService.AtualizarAsync(id, dto, vendedor, imagem);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id:guid}")]
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
        public IActionResult ObterImagem(string nome)
        {
            var caminho = Path.Combine(_env.WebRootPath, "images", "produtos", nome);

            if (!System.IO.File.Exists(caminho))
                return NotFound("Imagem não encontrada.");

            var contentType = GetContentType(caminho);

            var imagem = System.IO.File.OpenRead(caminho);
            return File(imagem, contentType);
        }
    }
}
