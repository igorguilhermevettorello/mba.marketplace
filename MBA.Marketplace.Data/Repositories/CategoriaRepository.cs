using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.DTOs;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Enums;
using MBA.Marketplace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBA.Marketplace.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
        public async Task<Categoria> CriarAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
        public async Task<Categoria> ObterPorIdAsync(Guid id)
        {
            return await _context.Categorias.FindAsync(id);
        }
        public async Task<bool> AtualizarAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task RemoverAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
