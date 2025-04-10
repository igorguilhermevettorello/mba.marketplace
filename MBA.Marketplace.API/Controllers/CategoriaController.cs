using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;
using MBA.Marketplace.Data.Extensions;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar(CategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _categoriaService.CriarAsync(dto);
            return CreatedAtAction(null, new { id = categoria.Id }, categoria);
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid id, CategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sucesso = await _categoriaService.AtualizarAsync(id, dto);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid id)
        {
            var status = await _categoriaService.RemoverAsync(id);
            if (status == StatusRemocaoEnum.NaoEncontrado)
                return NotFound();

            if (status == StatusRemocaoEnum.VinculacaoProduto)
            {
                var mensagem = status.GetDescription();
                return Conflict(new { mensagem });
            }
            
            return NoContent();
        }
    }
}
