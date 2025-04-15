using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Repositories.Interfaces;
using MBA.Marketplace.Data.Services.Interfaces;

namespace MBA.Marketplace.Data.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<Vendedor?> ObterPorIdAsync(string id)
        {
            return await _vendedorRepository.ObterPorUsuarioIdAsync(id);
                
        }
    }
}
