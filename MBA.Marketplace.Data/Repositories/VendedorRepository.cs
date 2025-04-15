using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBA.Marketplace.Data.Repositories
{
    public class VendedorRepository : IVendedorRepository
    {
        private readonly ApplicationDbContext _context;
        public VendedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CriarAsync(Vendedor vendedor)
        {
            _context.Vendedores.Add(vendedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Vendedor?> ObterPorUsuarioIdAsync(string usuario)
        {
            return await _context
                .Vendedores
                .Where(v => v.UsuarioId == usuario)
                .FirstOrDefaultAsync();
        }
    }
}
