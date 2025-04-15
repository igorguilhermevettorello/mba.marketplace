using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBA.Marketplace.Data.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly ApplicationDbContext _context;

        public VendedorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vendedor> ObterPorIdAsync(string id)
        {
            return await _context
                .Vendedores
                .Where(v => v.UsuarioId == id.ToString())
                .FirstOrDefaultAsync();
        }
    }
}
