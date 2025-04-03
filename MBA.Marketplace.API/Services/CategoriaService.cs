using MBA.Marketplace.API.Services.Interfaces;
using MBA.Marketplace.Core.DTOs;
using MBA.Marketplace.Core.Entities;
using MBA.Marketplace.Core.Enums;
using MBA.Marketplace.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MBA.Marketplace.API.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;
        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
        public async Task<Categoria> CriarAsync(CategoriaDto dto)
        {
            var categoria = new Categoria
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                CreatedAt = DateTime.Now
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
        public async Task<Categoria> ObterPorIdAsync(Guid id)
        {
            return await _context.Categorias.FindAsync(id);
        }
        public async Task<bool> AtualizarAsync(Guid id, CategoriaDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return false;

            categoria.Nome = dto.Nome;
            categoria.Descricao = dto.Descricao;
            categoria.UpdatedAt = DateTime.Now;

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<StatusRemocaoEnum> RemoverAsync(Guid id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return StatusRemocaoEnum.NaoEncontrado;

            var produto = await _context.Produtos.Where(p => p.CategoriaId == id).ToListAsync();
            if (produto.Any())
                return StatusRemocaoEnum.VinculacaoProduto;

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return StatusRemocaoEnum.Removido;
        }
    }
}
