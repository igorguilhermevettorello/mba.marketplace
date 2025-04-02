using MBA.Marketplace.API.Services;
using MBA.Marketplace.API.Services.Interfaces;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Data.Data;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<IActionResult> Criar([FromForm] ProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(v => v.UsuarioId == userId);

            if (vendedor == null)
                return Unauthorized("Usuário não é um vendedor válido.");

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);

            if (categoria == null)
                return BadRequest("Categoria não é válido.");

            var produto = await _produtoService.CriarAsync(dto, vendedor);

            return CreatedAtAction(null, new { id = produto.Id }, produto);
        }
    }
}
