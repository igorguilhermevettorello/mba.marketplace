using MBA.Marketplace.Data.Entities;

namespace MBA.Marketplace.Data.Services.Interfaces
{
    public interface IVendedorService
    {
        Task<Vendedor> ObterPorIdAsync(string id);
    }
}
