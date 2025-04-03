using MBA.Marketplace.API.Services;
using MBA.Marketplace.API.Services.Interfaces;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
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

        public ProdutoController(ApplicationDbContext context, IProdutoService produtoService)
        {
            _context = context;
            _produtoService = produtoService;
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
        public async Task<IActionResult> Atualizar(Guid id, ProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendedor = await BuscarVendedorLogado();

            var sucesso = await _produtoService.AtualizarAsync(id, dto, vendedor);
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
    }
}
