using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Enums;
using MBA.Marketplace.Core.Extensions;
using MBA.Marketplace.Infra.Services.Interfaces;
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
        public async Task<IActionResult> Criar(CategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _categoriaService.CriarAsync(dto);
            return CreatedAtAction(null, new { id = categoria.Id }, categoria);
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }
        [HttpPut("{id:guid}")]
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
